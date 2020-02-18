angular.module("goodApp")
    .controller("ProfileCtrl", [
        '$scope','$document', '$rootScope', '$http', '$cookies', 'appConfigFactory',
        function($scope,$document, $rootScope, $http, $cookies, appConfigFactory) {

            $rootScope.parentPath = '';
            $scope.loadingProfile = true;

            $scope.displayTimeSpan = function(date) {
                var createDate = moment(date);
                return createDate.fromNow();
            };

            $scope.pickNewPhoto = function() {
                $("#photoPicker").click();
            };

            var uploadNewPhoto = function() {
                $scope.changingPhoto = true;
                var formData = new FormData();
                formData.append('photo', $("#photoPicker")[0].files[0]);
                $http.post(appConfigFactory.apiUrl + 'api/users/changephoto', formData, {
                        transformRequest: angular.identity,
                        headers: {
                            "Content-Type": undefined
                        }
                    })
                    .success(function(data) {
                        $scope.profile.photoPath = data.photoPath;
                        saveUserInfo('photoPath', data.photoPath, false, '');
                        $scope.changingPhoto = false;
                    }).error(function(data) {
                        $scope.changingPhoto = false;
                    });
            };

            var setCookie = function(name, value, expires) {
                $document[0].cookie = escape(name) + '=' + escape(value) + ';path=/;expires=' + expires;
            };

            var saveUserInfo = function(name, value, isPersistent, expires) {
                $rootScope[name] = value;
                if (isPersistent) {
                    setCookie(name, value, expires);
                } else {
                    setCookie(name, value);
                }
            };

            $("#photoPicker").change(function() {
                if ($("#photoPicker").val() && !$scope.changingPhoto) {
                    uploadNewPhoto();
                }
            });

            var loadProfile = function() {
                $http.get(appConfigFactory.apiUrl + "api/users/me").success(function(data) {
                    $scope.profile = data;
                    $scope.loadingProfile = false;
                });
            };

            var loadOverview = function() {
                $http.get(appConfigFactory.apiUrl + "api/deeds/overview").success(function(data) {
                    $scope.overview = data;
                });
            };

            loadProfile();
            loadOverview();
        }
    ]);

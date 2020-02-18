'use strict';

angular.module('goodApp')
    .controller('LoginCtrl', [
        '$scope', '$http', '$document', '$rootScope', '$location', 'appConfigFactory',
        function($scope, $http, $document, $rootScope, $location, appConfigFactory) {

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

            $rootScope.parentPath = '';
            $scope.submitting = false;
            $scope.submit = function() {

                if ($scope.form.$invalid) {
                    return;
                }

                var data = $.param({
                    username: $scope.email,
                    password: $scope.password,
                    'grant_type': 'password'
                });

                $scope.submitting = true;
                $http.post(appConfigFactory.apiUrl + 'api/token', data)
                    .success(function(data) {
                        saveUserInfo('accessToken', data['access_token'], $scope.rememberMe, data['.expires']);
                        saveUserInfo('name', data.name, $scope.rememberMe, data['.expires']);
                        saveUserInfo('joinedGroup', data.joinedGroup === 'true', $scope.rememberMe, data['.expires']);
                        saveUserInfo('photoPath', data.photoPath, $scope.rememberMe, data['.expires']);

                        $http.defaults.headers.common.Authorization = 'bearer ' + data['access_token'];
                        if (data.joinedGroup !== 'true') {
                            $location.path('/joinGroup');
                        } else {
                            $location.path('/main');
                        }
                        $scope.submitting = false;
                    })
                    .error(function(data, status) {
                        if (status === 400) {
                            $scope.loginError = data['error_description'];
                        }
                        $scope.submitting = false;
                    });
            };
        }
    ]);

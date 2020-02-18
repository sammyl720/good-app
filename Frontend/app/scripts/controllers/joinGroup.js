'use strict';

angular.module('goodApp')
    .controller('JoinGroupCtrl', ['$scope', '$http', '$rootScope', '$cookies', '$location', 'appConfigFactory', function($scope, $http, $rootScope, $cookies, $location, appConfigFactory) {
        $scope.submitting = false;
        $scope.submit = function() {
            if ($scope.form.$invalid) {
                return;
            }
            $scope.submitting = true;
            $http.post(appConfigFactory.apiUrl + 'api/groups/' + encodeURIComponent($scope.code) + '/join')
                .success(function() {
                    $rootScope.joinedGroup = 'true';
                    $cookies.joinedGroup = 'true';
                    $location.path('/main');
                    $scope.submitting = false;
                })
                .error(function(data) {
                    $scope.errorMessage = data.message;
                    $scope.submitting = false;
                });
        };
    }]);

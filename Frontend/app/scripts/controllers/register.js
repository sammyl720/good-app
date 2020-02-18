'use strict';

/**
 * @ngdoc function
 * @name goodApp.controller:RegisterCtrl
 * @description
 * # RegisterCtrl
 * Controller of the goodApp
 */
angular.module('goodApp')
    .controller('RegisterCtrl', [
        '$scope', '$http', 'appConfigFactory', '$rootScope', function($scope, $http, appConfigFactory, $rootScope) {
            $scope.submitting = false;
            $rootScope.parentPath = '#/login';

            $scope.submit = function() {
                if ($scope.form.$invalid) {
                    return;
                }

                var formData = new FormData();
                formData.append('email', $scope.email);
                formData.append('password', $scope.password);
                formData.append('confirmPassword', $scope.confirmPassword);
                formData.append('firstName', $scope.firstName);
                formData.append('lastName', $scope.lastName);
                formData.append('photo', document.getElementById('photo').files[0]);
                $scope.submitting = true;
                $http.post(
                        appConfigFactory.apiUrl + 'api/account/register', formData, {
                            transformRequest: angular.identity,
                            headers: {
                                'Content-Type': undefined
                            }
                        })
                    .success(function() {
                        $scope.registered = true;
                        $scope.submitting = false;
                    })
                    .error(function(data) {
                        $scope.registerError = data.modelState[''][0];
                        $scope.submitting = false;
                    });
            };
        }
    ]);

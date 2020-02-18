'use strict';

/**
 * @ngdoc function
 * @name goodApp.controller:MyDeedsCtrl
 * @description
 * # MyDeedsCtrl
 * Controller of the goodApp
 */
angular.module('goodApp')
    .controller('MyDeedsCtrl', [
        '$scope', '$http', '$rootScope', '$location', 'appConfigFactory', function($scope, $http, $rootScope, $location, appConfigFactory) {
            $rootScope.parentPath = '';
            $scope.name = $rootScope.name;
            $scope.photoPath = $rootScope.photoPath;
            $scope.challenges = [];
            $scope.loadingChallenges = true;
            $scope.submitDeed = function(id) {
                $location.path('/challenges/' + id + '/newDeed');
            };

            var loadChallenges = function() {
                $scope.loadingChallenges = true;
                $http.get(appConfigFactory.apiUrl + 'api/challenges')
                    .success(function(data) {
                        $scope.challenges = data;
                        $scope.loadingChallenges = false;
                    });
            };

            var loadDeedOverView = function() {
                $http.get(appConfigFactory.apiUrl + 'api/deeds/overview')
                    .success(function(data) {
                        $scope.deedCounts = data;
                    });
            };

            loadChallenges();
            loadDeedOverView();
        }
    ]);
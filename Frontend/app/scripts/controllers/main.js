'use strict';

/**
 * @ngdoc function
 * @name goodApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the goodApp
 */
angular.module('goodApp')
    .controller('MainCtrl', [
        '$scope', '$document', '$http', '$rootScope', '$location', 'appConfigFactory',
        function($scope, $document, $http, $rootScope, $location, appConfigFactory) {
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

            var loadGroupInfo = function() {
                $http.get(appConfigFactory.apiUrl + 'api/groups/mine')
                    .success(function(data) {
                        $rootScope.groupPicturePath = data.picture;
                        $document[0].cookie = escape("groupPicturePath") + '=' + escape(data.picture) + ';path=/';
                    });
            };

            loadChallenges();
            loadDeedOverView();
            loadGroupInfo();
        }
    ]);

'use strict';

angular.module('goodApp')
    .controller('ChallengeCtrl', [
        '$scope', '$route', '$rootScope', '$routeParams', '$http', '$anchorScroll', '$location', '$document', 'appConfigFactory',
        function($scope, $route, $rootScope, $routeParams, $http, $anchorScroll, $location, $document, appConfigFactory) {
            $scope.challenge = {};
            $scope.comments = [];
            $scope.loadingChallenge = true;
            $scope.loadingComments = true;
            $scope.submittingCaption = false;

            $scope.scrollToBottom = function() {
                // $location.hash('bottom');
                // $anchorScroll();
                $('body').scrollTop($('body')[0].scrollHeight + 500);
            };

            $scope.getRemainTime = function() {
                var dueDate = moment($scope.challenge.dueDate);
                return dueDate.fromNow(true);
            };

            $scope.addNewCaption = function() {
                if (!$scope.caption) {
                    return;
                }
                var data = $.param({
                    caption: $scope.caption
                });
                $scope.submittingCaption = true;
                $http.post(appConfigFactory.apiUrl + "api/challenges/" + $routeParams.challengeId + "/comments", data)
                    .success(function() {
                        getComments();
                    }).error(function() {
                        $scope.submittingCaption = false;
                    });
            };

            $http.get(appConfigFactory.apiUrl + "api/challenges/" + $routeParams.challengeId)
                .success(function(data) {
                    $scope.challenge = data;
                    $scope.rating = data.rating;
                    $rootScope.parentPath = '#/';
                    $scope.loadingChallenge = false;
                });

            var getComments = function() {
                $http.get(appConfigFactory.apiUrl + "api/challenges/" + $routeParams.challengeId + "/comments")
                    .success(function(data) {
                        $scope.comments = data;
                        $scope.loadingComments = false;

                        if ($scope.submittingCaption) {
                            $scope.submittingCaption = false;
                            setTimeout(function() {
                                $scope.scrollToBottom();
                            }, 1);
                        }
                    });
            };

            getComments();
        }
    ]);

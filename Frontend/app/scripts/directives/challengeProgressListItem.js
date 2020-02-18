angular.module('goodApp')
    .directive('challengeProgressListItem', [
        function() {
            return {
                scope: {
                    name: '=name',
                    completedCount: '=completedCount',
                    count: '=count',
                    challengeId: '=challengeId',
                    dueDate: '=dueDate',
                    takeAction: '=takeAction',
                    picture: '=picture'
                },
                controller: function($scope) {
                    var getPercent = function() {
                        if ($scope.completedCount && $scope.count) {
                            var percent = $scope.completedCount / $scope.count * 100;
                            if (percent > 100) {
                                return 100;
                            }
                            return percent;
                        } else {
                            return 0;
                        }
                    };

                    $scope.progressStyle = {
                        'width': getPercent() + '%',
                        'background-color': getPercent() === 100 ? '#4CAF50' : '#FFA000'
                    };

                    $scope.showDetails = function() {
                        if ($scope.takeAction) {
                            location.href = '#/challenges/' + $scope.challengeId + '/newDeed';
                        } else {
                            location.href = '#/challenges/' + $scope.challengeId;
                        }
                    };

                    $scope.getProgressText = function() {
                        if ($scope.count <= $scope.completedCount) {
                            return 'Completed';
                        }
                        return $scope.completedCount + '/' + $scope.count;
                    };

                    $scope.getCompleted = function() {
                        return $scope.count <= $scope.completedCount;
                    };

                    $scope.getRemainTime = function() {
                        var dueDate = moment($scope.dueDate);
                        return dueDate.fromNow(true);
                    };
                },
                templateUrl: 'views/challengeProgressListItem.html'
            }
        }
    ]);

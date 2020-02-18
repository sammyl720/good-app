'use strict';

angular.module('goodApp')
    .controller('NewDeedCtrl', [
        '$scope', '$rootScope', '$http', '$document', '$routeParams', '$location', 'appConfigFactory',
        function($scope, $rootScope, $http, $document, $routeParams, $location, appConfigFactory) {
            $rootScope.parentPath = '#/';
            $scope.location = '';
            $scope.loadingLocation = false;
            var now = new Date();
            $scope.time = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes());
            $scope.rating = 3;
            $scope.lat = '';
            $scope.lon = '';
            $scope.recipients = [];
            $scope.selectedRecipients = [];
            $scope.loadingRecipients = true;
            $scope.loadingChallenge = true;
            $scope.searchKey = '';
            $scope.showStranger = true;

            $http.get(appConfigFactory.apiUrl + "api/challenges/" + $routeParams.challengeId).success(function(data) {
                $scope.title = data.name;
                $scope.loadingChallenge = false;
            });

            var loadRecipients = function(q, pageIndex, pageSize, callback) {

                if (!pageIndex) {
                    pageIndex = 0;
                }

                if (!pageSize) {
                    pageSize = 5;
                }

                var url = appConfigFactory.apiUrl + "api/users/ingroup?pageIndex=" + pageIndex + "&pageSize=" + pageSize;
                if (q) {
                    url += "&q=" + q;
                }


                $http.get(url).success(function(data) {
                    if (callback) {
                        callback(data);
                    }
                });
            };

            $scope.refreshRecipients = function(q) {
                $scope.recipients.length = 0;
                loadRecipients(q, 0, 5, function(data) {
                    var result = [];
                    if (data.length > 0) {
                        for (var i = 0; i < data.length; i++) {
                            if (!checkIfSelected(data[i])) {
                                result.push(data[i]);
                            }
                        }
                    }
                    $scope.recipients = result;
                });
            };

            var checkIfSelected = function(recipient) {
                if ($scope.selectedRecipients.length > 0) {
                    for (var i = 0; i < $scope.selectedRecipients.length; i++) {
                        if (recipient.userId === $scope.selectedRecipients[i].userId) {
                            return true;
                        }
                    }
                }
                return false;
            };

            $scope.selectRecipient = function(recipient) {
                if (recipient.selected)
                    recipient.selected = false;
                else
                    recipient.selected = true;

                checkIfShowStranger();
            };

            $scope.someFunction = function(item, model) {
                model.selected = true;
                $scope.selectedRecipients.push(model);
                $scope.recipients.splice($scope.recipients.indexOf(model), 1);
            };

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function(position) {
                    $scope.$apply(function() {
                        var pyrmont = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
                        $scope.lat = position.coords.latitude;
                        $scope.lon = position.coords.longitude;
                        var request = {
                            location: pyrmont,
                            radius: '500'
                        };

                        var mapPlaceHolder = document.querySelector('#mapPlaceHolder');
                        var service = new google.maps.places.PlacesService(mapPlaceHolder);

                        if (service) {
                            service.nearbySearch(request, function(results, status) {
                                if (status === google.maps.places.PlacesServiceStatus.OK) {
                                    $scope.$apply(function() {
                                        if (results && results.length > 0) {
                                            for (var i = 0; i < results.length; i++) {
                                                if (results[i].vicinity) {
                                                    $scope.location = results[i].vicinity;
                                                    return;
                                                }
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    });
                });
            }

            var checkIfShowStranger = function() {
                var result = true;
                if ($scope.selectedRecipients.length > 0) {
                    for (var i = 0; i < $scope.selectedRecipients.length; i++) {
                        if ($scope.selectedRecipients[i].selected) {
                            result = false;
                        }
                    }
                }

                $scope.showStranger = result;
            };

            $scope.submit = function() {

                if ($scope.form.$invalid) {
                    return;
                }

                $scope.submittingNewDeed = true;
                var taggedUserIds = [];
                if ($scope.selectedRecipients.length > 0) {
                    for (var i = 0; i < $scope.selectedRecipients.length; i++) {
                        if ($scope.selectedRecipients[i].selected) {
                            taggedUserIds.push($scope.selectedRecipients[i].userId);
                        }
                    }
                }

                $http.post(appConfigFactory.apiUrl + "api/challenges/" + $routeParams.challengeId + "/deeds", $.param({
                        deedTime: $scope.time.toUTCString(),
                        location: $scope.location,
                        lat: $scope.lat,
                        lon: $scope.lon,
                        rating: $scope.rating,
                        comment: $scope.comment,
                        taggedUserIds: taggedUserIds
                    }))
                    .success(function(data) {
                        $location.path('/');
                        $scope.submittingNewDeed = false;
                    })
                    .error(function(data) {
                        $scope.errorMessage = data.message;
                        $scope.submittingNewDeed = false;
                    });
            };

            loadRecipients(null, 0, 5, function(data) {
                $scope.selectedRecipients = data;
            });
        }
    ]);

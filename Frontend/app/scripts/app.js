'use strict';

angular.module('goodApp', [
        'ngRoute',
        'ngCookies',
        'ngMessages',
        'ui.bootstrap',
        'ngSanitize',
        'ui.select'
    ])
    .factory('appConfigFactory', [
        function() {
            return {
                apiUrl: 'http://localhost:63990/'
                // apiUrl: 'http://goodappbackend.azurewebsites.net/'
            };
        }
    ])
    .factory('unauthorizedInterceptor', [
        '$q',
        function($q) {
            var requestInterceptor = {
                'responseError': function(rejectReason) {
                    if (rejectReason.status === 401) {
                        location.href = '#/login';
                    }
                    return $q.reject(rejectReason);
                }
            };
            return requestInterceptor;
        }
    ])
    .config([
        '$routeProvider',
        function($routeProvider) {
            $routeProvider
                .when('/', {
                    templateUrl: 'views/main.html',
                    controller: 'MainCtrl'
                })
                .when('/login', {
                    templateUrl: 'views/login.html',
                    controller: 'LoginCtrl'
                })
                .when('/register', {
                    templateUrl: 'views/register.html',
                    controller: 'RegisterCtrl'
                })
                .when('/joinGroup', {
                    templateUrl: 'views/joinGroup.html',
                    controller: 'JoinGroupCtrl'
                })
                .when('/challenges/:challengeId', {
                    templateUrl: 'views/challenge.html',
                    controller: 'ChallengeCtrl'
                })
                .when('/challenges/:challengeId/newDeed', {
                    templateUrl: 'views/newDeed.html',
                    controller: 'NewDeedCtrl'
                })
                .when('/profile', {
                    templateUrl: 'views/profile.html',
                    controller: 'ProfileCtrl'
                })
                .when('/myDeeds', {
                    templateUrl: 'views/myDeeds.html',
                    controller: 'MyDeedsCtrl'
                })
                .otherwise({
                    redirectTo: '/'
                });
        }
    ])
    .config([
        '$httpProvider',
        function($httpProvider) {
            $httpProvider.interceptors.push('unauthorizedInterceptor');
        }
    ])
    .run([
        '$http', '$cookies', '$document', '$rootScope', '$location',
        function($http, $cookies, $document, $rootScope, $location) {

            $('.nav a').on('click', function() {
                $(".btn-navbar").click(); //bootstrap 2.x
                $(".navbar-toggle").click(); //bootstrap 3.x by Richard
            });

            $http.defaults.headers.common.appToken = 'DD8563F1288A4769B9917B4F6219666B';
            $http.defaults.headers.post = {
                'Content-Type': 'application/x-www-form-urlencoded'
            };
            $http.defaults.headers.put = {
                'Content-Type': 'application/x-www-form-urlencoded'
            };

            $rootScope.showTitleBackArrow = function() {
                return $rootScope.parentPath !== '';
            };

            $rootScope.getParentPath = function() {
                if ($rootScope.parentPath === '') {
                    return '#/';
                }

                return $rootScope.parentPath;
            };

            $rootScope.logout = function() {
                $document[0].cookie = 'accessToken=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                $document[0].cookie = 'joinedGroup=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                $document[0].cookie = 'name=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                $document[0].cookie = 'photoPath=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                $document[0].cookie = 'groupPicturePath=;path=/;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                $rootScope.accessToken = '';
                $rootScope.joinedGroup = false;
                $rootScope.name = '';
                $rootScope.photoPath = '';
                $rootScope.groupPicturePath = '';
                $http.defaults.headers.common.Authorization = '';
                $location.path('/login');
            };

            $rootScope.showLogout = function() {
                return $cookies.accessToken;
            };

            $rootScope.showGroupPicture = function() {
                return $rootScope.groupPicturePath != undefined && $rootScope.groupPicturePath != '';
            }

            var blueBackgroundViews = ['views/challenge.html', 'views/myDeeds.html'];

            $rootScope.$on('$routeChangeStart', function(event, next) {

                $('body').removeClass('blue-background');

                if (blueBackgroundViews.indexOf(next.templateUrl) >= 0) {
                    $('body').addClass('blue-background');
                } else {
                    $('body').removeClass('blue-background');
                }

                if (!$rootScope.accessToken) {
                    if (next.templateUrl !== 'views/login.html' && next.templateUrl !== 'views/register.html') {
                        $location.path('/login');
                        return;
                    }
                }

                if (!$rootScope.joinedGroup) {
                    if (next.templateUrl !== 'views/login.html' && next.templateUrl !== 'views/register.html' && next.templateUrl !== 'views/joinGroup.html') {
                        $location.path('/joinGroup');
                        return;
                    }
                }
            });

            if ($cookies.accessToken) {
                $http.defaults.headers.common.Authorization = 'bearer ' + $cookies.accessToken;
                $rootScope.accessToken = $cookies.accessToken;
                $rootScope.joinedGroup = $cookies.joinedGroup;
                $rootScope.name = $cookies.name;
                $rootScope.photoPath = $cookies.photoPath;
                $rootScope.groupPicturePath = $cookies.groupPicturePath;

                if ($rootScope.joinedGroup !== 'true') {
                    $location.path('/joinGroup');
                }
            } else {
                $location.path('/login');
            }
        }
    ]);

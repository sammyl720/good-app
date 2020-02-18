'use strict';

angular.module('goodApp')
    .directive('equalsTo', [function() {

        var link = function($scope, $element, $attrs, ctrl) {

            var validate = function(viewValue) {
                var comparisonModel = $attrs.equalsTo;

                if (!viewValue || !comparisonModel) {
                    ctrl.$setValidity('equalsTo', true);
                }
                ctrl.$setValidity('equalsTo', viewValue === comparisonModel);
                return viewValue;
            };

            ctrl.$parsers.unshift(validate);
            ctrl.$formatters.push(validate);

            $attrs.$observe('equalsTo', function() {
                return validate(ctrl.$viewValue);
            });
        };

        return {
            require: 'ngModel',
            link: link
        };

    }])
    .directive('validFile', function() {
        return {
            require: 'ngModel',
            link: function(scope, element, attrs, ngModel) {
                ngModel.$render = function() {
                    ngModel.$setViewValue(element.val());
                };

                element.bind('change', function() {
                    scope.$apply(function() {
                        ngModel.$render();
                    });
                });
            }
        };
    });

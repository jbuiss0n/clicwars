'use strict';

var INTEGER_REGEXP = /^\-?\d+$/;

var app = angular.module('app');

app.directive('integer', function () {
  return {
    require: 'ngModel',
    link: function (scope, elm, attrs, ctrl) {
      ctrl.$parsers.unshift(function (viewValue) {
        if (INTEGER_REGEXP.test(viewValue)) {
          ctrl.$setValidity('integer', true);
          return viewValue;
        } else {
          ctrl.$setValidity('integer', false);
          return undefined;
        }
      });
    }
  };
});
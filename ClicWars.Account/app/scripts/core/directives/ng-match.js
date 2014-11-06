'use strict';

angular
  .module('clicwars.account')
  .directive('ngMatch', function() {
    return {
      require: 'ngModel',
      restrict: 'A',
      scope: {
        ngMatch: '='
      },
      link: function(scope, elem, attrs, ctrl) {
        scope.$watch(function() {
          var modelValue = ctrl.$modelValue || ctrl.$$invalidModelValue;
          return (ctrl.$pristine && angular.isUndefined(modelValue)) || scope.ngMatch === modelValue;
        }, function(currentValue) {
          ctrl.$setValidity('match', currentValue);
        });
      }
    };
  });
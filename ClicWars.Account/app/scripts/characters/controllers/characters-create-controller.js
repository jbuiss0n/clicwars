'use strict';

angular
  .module('clicwars.account.characters')
  .controller('CharactersCreateController', [
    '$scope',
    '$interval',
    'CharacterService',
    function($scope, $interval, CharacterService) {
      var self = this, _x = 0;

      $scope.body = 0;
      $scope.name = '';

      self.onCreate = function() {
        CharacterService.create($scope.name, $scope.body, function(result) {
          $scope.$state.go('characters.list');
        });
      };

      self.onPrev = function() {
        if ($scope.body > 0)
          $scope.body--;
      };

      self.onNext = function() {
        if ($scope.body < 53)
          $scope.body++;
      };

      self.getFrontBodyPosition = function() {
        var x = ($scope.body % 16) * -64 - _x;
        var y = (Math.floor($scope.body / 16) * -128) - 64;
        return x + 'px ' + y + 'px';
      };

      self.getRightBodyPosition = function() {
        var x = ($scope.body % 16) * -64 - _x;
        var y = (Math.floor($scope.body / 16) * -128) - 32;
        return x + 'px ' + y + 'px';
      };

      self.getBackBodyPosition = function() {
        var x = ($scope.body % 16) * -64 - _x;
        var y = (Math.floor($scope.body / 16) * -128);
        return x + 'px ' + y + 'px';
      };

      self.getLeftBodyPosition = function() {
        var x = ($scope.body % 16) * -64 - _x;
        var y = (Math.floor($scope.body / 16) * -128) - 96;
        return x + 'px ' + y + 'px';
      };

      $interval(function() {
        _x = (_x == 0 ? 32 : 0);
      }, 500);
    }]);
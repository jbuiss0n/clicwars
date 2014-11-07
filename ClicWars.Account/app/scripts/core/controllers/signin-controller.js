'use strict';

angular
  .module('clicwars.account')
  .controller('SigninController', [
    '$scope',
    'AccountService',
    'AuthService',
    function($scope, AccountService, AuthService) {
      var self = this;

      var onCreateSuccess = function(result) {
        AuthService.authenticate(result);
        $scope.$state.go('characters.list');
      };

      var onCreateError = function(error) {
        $scope.error = error;
      };

      self.onSignin = function() {
        AccountService.create(
          { username: $scope.username, password: $scope.password },
          onCreateSuccess,
          onCreateError);
      };
    }]);
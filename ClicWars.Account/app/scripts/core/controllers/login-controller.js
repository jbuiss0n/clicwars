'use strict';

angular
  .module('clicwars.account')
  .controller('LoginController', [
    '$scope',
    'AccountService',
    'AuthService',
    function($scope, AccountService, AuthService) {
      var self = this;

      var onLoginSuccess = function(result) {
        AuthService.authenticate(result);
        $scope.$state.go('characters.list');
      };

      var onLoginError = function(error) {
        $scope.error = error;
      };

      self.onLogin = function() {
        AccountService.login(
          { Username: $scope.username, Password: $scope.password },
          onLoginSuccess,
          onLoginError);
      };
    }]);
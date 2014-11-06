'use strict';

var app = angular.module('app');

app.controller('CreateController', ['$rootScope', '$scope', '$location', 'AccountService', function ($rootScope, $scope, $location, AccountService) {

  $scope.createAccount = function (account) {
    AccountService.save({ Username: account.username, Body: account.body }, function (res) {
      $rootScope.username = res.Username;
      $rootScope.serial = res.Serial;

      $location.path("/game");
    });
  };

}]);
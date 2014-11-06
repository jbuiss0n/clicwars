'use strict';

var app = angular.module('app', [
  'ngRoute',
  'ngResource',
  'luegg.directives'
]);

app.config(['$routeProvider', function ($routeProvider) {

  $routeProvider
    .when('/', {
      templateUrl: '/views/index',
      controller: 'HomeController'
    })
    .when('/create', {
      templateUrl: '/views/create',
      controller: 'CreateController'
    })
    .when('/game', {
      templateUrl: '/views/game',
      controller: 'GameController'
    })
    .when('/death', {
      templateUrl: '/views/death',
    })
  .otherwise({
    templateUrl: '/views/404',
  });

}]);
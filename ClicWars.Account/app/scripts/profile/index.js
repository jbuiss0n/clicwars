'use strict';

angular
  .module('clicwars.account.profile', [
    'ui.router',
    'ngResource'
  ])

  .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
    $stateProvider
      .state('profile', {
        url: '/profile',
        templateUrl: 'views/profile/index.html',
        controller: 'ProfileController',
        data: {
          authenticate: true
        }
      });
  }]);
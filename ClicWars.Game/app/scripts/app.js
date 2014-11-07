'use strict';

angular
  .module('clicwars.game', [
    'ui.router',
    'ngResource',
    'ipCookie'
  ])

  .run(['$rootScope', '$state', '$stateParams', '$location', 'PageService', function($rootScope, $state, $stateParams, $location, PageService) {
    $rootScope.$state = $state;
    $rootScope.$location = $location;
    $rootScope.$stateParams = $stateParams;

    $rootScope.Page = PageService;
  }])

  .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/');

    $stateProvider
      .state('game', {
        abstract: true,
        url: '/',
        templateUrl: 'views/game.html',
        controller: 'GameController'
      })
      .state('game.main', {
        url: '',
        views: {
          'screen': {
            templateUrl: 'views/screen/main.html',
            controller: 'ScreenController as screen'
          },
          'player': {
            templateUrl: 'views/partials/player.html',
            controller: 'PlayerController'
          },
          'chat': {
            template: 'Chat here. Work in progress...'
          }
        }
      });
  }]);
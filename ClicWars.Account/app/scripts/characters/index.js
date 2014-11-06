'use strict';

angular
  .module('clicwars.account.characters', [
    'ui.router',
    'ngResource'
  ])

  .config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
    $stateProvider
      .state('characters', {
        abstract: true,
        url: '/characters',
        templateUrl: 'views/characters/index.html',
        controller: 'CharactersController',
        data: {
          authenticate: true
        }
      })
      .state('characters.list', {
        url: '',
        templateUrl: 'views/characters/list.html',
        controller: 'CharactersListController as list',
      })
      .state('characters.create', {
        url: '/create',
        templateUrl: 'views/characters/create.html',
        controller: 'CharactersCreateController as create',
      });
  }]);
'use strict';

angular
  .module('clicwars.account', [
    'ui.router',
    'ngResource',
    'ngMessages',
    'clicwars.account.profile',
    'clicwars.account.characters'
  ])

  .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', function($stateProvider, $urlRouterProvider, $httpProvider) {
    $urlRouterProvider.otherwise('/login');

    $stateProvider
      .state('signin', {
        url: '/signin',
        templateUrl: 'views/signin.html',
        controller: 'SigninController as signin'
      })
      .state('login', {
        url: '/login',
        templateUrl: 'views/login.html',
        controller: 'LoginController as login'
      });

    $httpProvider.interceptors.push(['$q', '$location', function($q, $location) {
      return {
        'responseError': function(response) {
          if (response.status === 401 || response.status === 403) {
            $location.path('/login');
          }
          return $q.reject(response);
        }
      };
    }]);
  }])

  .run(['$rootScope', '$state', '$stateParams', 'PageService', 'AuthService', function($rootScope, $state, $stateParams, PageService, AuthService) {
    $rootScope.$on('$stateChangeStart', function(event, toState, toParams, fromState, fromParams) {
      if (toState.data && toState.data.authenticate && !AuthService.isAuthenticated()) {
        $state.go('login');
        event.preventDefault();
      }
    });
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    $rootScope.Page = PageService;
  }]);
'use strict';

angular
  .module('clicwars.account')
  .factory('GameService', [
    '$resource',
    'AuthService',
    function($resource, AuthService) {
      var resource = $resource(CONFIG.BASE_API_URL + '/Game');

      return {
        play: function(serial, cb) {
          return resource.get({ username: AuthService.username(), token: AuthService.token(), serial: serial }, cb);
        }
      };
    }]);

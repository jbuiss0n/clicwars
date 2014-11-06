'use strict';

angular
  .module('clicwars.account.characters')
  .factory('CharacterService', [
    '$resource',
    'AuthService',
    function($resource, AuthService) {
      var resource = $resource(CONFIG.BASE_API_URL + '/Characters');

      return {
        query: function(cb) {
          return resource.query({ username: AuthService.username(), token: AuthService.token() }, cb);
        },
        create: function(name, body, cb) {
          return resource.save({ username: AuthService.username(), token: AuthService.token() }, { Name: name, Body: body }, cb);
        }
      };
    }]);
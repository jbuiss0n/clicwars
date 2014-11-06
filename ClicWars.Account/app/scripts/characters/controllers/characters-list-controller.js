'use strict';

angular
  .module('clicwars.account.characters')
  .controller('CharactersListController', [
    '$scope',
    '$interval',
    '$cookieStore',
    'AuthService',
    'GameService',
    'CharacterService',
    function($scope, $interval, $cookieStore, AuthService, GameService, CharacterService) {
      var self = this, _x = 0;

      self.getBodyPosition = function(body) {
        var x = (body % 16) * -64 - _x;
        var y = (Math.floor(body / 16) * -128) - 64;
        return x + 'px ' + y + 'px';
      };

      self.play = function(serial) {
        GameService.play(serial, function(result) {
          $cookieStore.put('game-token', { username: AuthService.username(), token: result.Value })
          window.location = CONFIG.BASE_GAME_URL;
        });
      };

      CharacterService.query(function(characters) {
        self.characters = characters;
      });

      $interval(function() {
        _x = (_x == 0 ? 32 : 0);
      }, 500);
    }]);
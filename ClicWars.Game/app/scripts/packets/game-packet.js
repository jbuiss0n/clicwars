'use strict';

angular
  .module('clicwars.game')
  .factory('GamePacket', ['PACKET', function(PACKET) {
    return {
      LoginPacket: function(username, token) {
        this.Id = PACKET.GAME_EVENT.LOGIN;
        this.Username = username;
        this.Token = token;
      }
    };
  }]);

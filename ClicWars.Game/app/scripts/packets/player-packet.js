'use strict';

angular
  .module('clicwars.game')
  .factory('PlayerPacket', ['PACKET', function(PACKET) {
    return {
      DeathPacket: function(serial) {
        this.Id = PACKET.PLAYER_EVENT.DEATH;
        this.Serial = serial;
      },
      RespawnRequestPacket: function(serial) {
        this.Id = PACKET.PLAYER_EVENT.RESPAWN;
        this.Serial = serial;
      }
    };
  }]);

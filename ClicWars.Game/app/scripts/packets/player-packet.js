'use strict';

angular
  .module('clicwars.game')
  .factory('PlayerPacket', ['PACKET', function(PACKET) {
    return {
      StatusPacket: function(serial, body, mana, hits, manaMax, hitsMax, speed, regenHits, regenMana) {
        this.Id = PACKET.PLAYER_EVENT.STATUS;
        this.Serial = serial;
        this.Body = body;
        this.Mana = mana;
        this.Hits = hits;
        this.ManaMax = manaMax;
        this.HitsMax = hitsMax;
        this.Speed = speed;
        this.RegenHits = regenHits;
        this.RegenMana = regenMana;
      },
      DeathPacket: function(serial) {
        this.Id = PACKET.PLAYER_EVENT.DEATH;
        this.Serial = serial;
      },
      RespawnRequestPacket: function() {
        this.Id = PACKET.PLAYER_EVENT.RESPAWN_REQUEST;
      },
    };
  }]);

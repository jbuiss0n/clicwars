'use strict';

angular
  .module('clicwars.game')
  .factory('WorldPacket', ['PACKET', function(PACKET) {
    return {
      EffectPacket: function(effect, x, y) {
        this.Id = PACKET.WORLD_EVENT.EFFECT;
        this.Effect = effect;
        this.X = x;
        this.Y = y;
      },
      ChatMessagePacket: function(type, from, message) {
        this.Id = PACKET.WORLD_EVENT.CHAT;
        this.Type = type;
        this.FromSerial = from;
        this.Message = message;
      }
    };
  }]);

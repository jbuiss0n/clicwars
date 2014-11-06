'use strict';

angular
  .module('clicwars.game')
  .factory('MobilePacket', ['PACKET', function(PACKET) {
    return {
      MovementRequestPacket: function(direction) {
        this.Id = PACKET.MOBILE_EVENT.MOVE_REQUEST;
        this.Direction = direction;
      },
      MovementRejectPacket: function(x, y, direction) {
        this.Id = PACKET.MOBILE_EVENT.MOVE_REJECT;
        this.X = x;
        this.Y = y;
        this.Direction = direction;
      },
      IncomingPacket: function(x, y, direction, body, serial) {
        this.Id = PACKET.MOBILE_EVENT.INCOMING;
        this.X = x;
        this.Y = y;
        this.Body = body;
        this.Serial = serial;
        this.Direction = direction;
      },
      RemovingPacket: function(serial) {
        this.Id = PACKET.MOBILE_EVENT.REMOVING;
        this.Serial = serial;
      },
      MovingPacket: function(x, y, direction, serial) {
        this.Id = PACKET.MOBILE_EVENT.MOVING;
        this.X = x;
        this.Y = y;
        this.Serial = serial;
        this.Direction = direction;
      },
      FireballPacket: function(x, y) {
        this.Id = PACKET.MOBILE_EVENT.FIREBALL;
        this.X = x;
        this.Y = y;
      }
    };
  }]);
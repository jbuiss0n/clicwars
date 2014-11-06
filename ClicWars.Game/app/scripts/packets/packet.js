'use strict';

angular
  .module('clicwars.game')
  .value('PACKET', {
    GAME_EVENT: {
      LOGIN: 0x1,
    },
    MAP_EVENT: {
      LOAD: 0x10,
    },
    MOBILE_EVENT: {
      MOVE_REQUEST: 0x20,
      MOVE_REJECT: 0x21,

      INCOMING: 0x30,
      REMOVING: 0x31,
      MOVING: 0x32,

      FIREBALL: 0x50,
    },
    PLAYER_EVENT: {
      STATUS: 0x40,
      DEATH: 0x41,
      RESPAWN_REQUEST: 0x42,
    },
    WORLD_EVENT: {
      EFFECT: 0x60,
      CHAT: 0x61
    }
  });
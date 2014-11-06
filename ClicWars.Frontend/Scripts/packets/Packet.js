var PACKET = {
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
    MOVE: 0x32,

    DEATH: 0x40,
    STATUS: 0x41,

    FIREBALL: 0x50,
  },

  WORLD_EVENT: {
    EFFECT: 0x60,
    CHAT: 0x61
  },

  PLAYER_EVENT: {
    DEATH: 0x70,
    RESPAWN: 0x71,
  }

};
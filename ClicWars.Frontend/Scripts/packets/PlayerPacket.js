var PlayerDeathPacket = function (serial) {
  this.Id = PACKET.PLAYER_EVENT.DEATH;
  this.Serial = serial;
};

var PlayerRespawnRequestPacket = function (serial) {
  this.Id = PACKET.PLAYER_EVENT.RESPAWN;
  this.Serial = serial;
};

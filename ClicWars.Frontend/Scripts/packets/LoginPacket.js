var LoginPacket = function (username, serial) {
  this.Id = PACKET.GAME_EVENT.LOGIN;
  this.Username = username;
  this.Serial = serial;
};

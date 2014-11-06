var MovementRequestPacket = function (direction) {
  this.Id = PACKET.MOBILE_EVENT.MOVE_REQUEST;
  this.Direction = direction;
};

var MovementRejectPacket = function (x, y, direction) {
  this.Id = PACKET.MOBILE_EVENT.MOVE_REJECT;
  this.X = x;
  this.Y = y;
  this.Direction = direction;
};

var MobileIncoming = function (x, y, direction, body, serial) {
  this.Id = PACKET.MOBILE_EVENT.INCOMING;
  this.X = x;
  this.Y = y;
  this.Body = body;
  this.Serial = serial;
  this.Direction = direction;
};

var MobileRemoving = function (serial) {
  this.Id = PACKET.MOBILE_EVENT.REMOVING;
  this.Serial = serial;
};

var MobileMovingPacket = function (x, y, direction, serial) {
  this.Id = PACKET.MOBILE_EVENT.MOVE;
  this.X = x;
  this.Y = y;
  this.Serial = serial;
  this.Direction = direction;
};

var FireballRequestPacket = function (x, y) {
  this.Id = PACKET.MOBILE_EVENT.FIREBALL;
  this.X = x;
  this.Y = y;
};
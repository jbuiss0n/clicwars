var Mobile = function(serial, body, image, position, direction) {
  this.initialize(serial, body, image, position, direction);
};
Mobile.prototype = new createjs.Sprite();
Mobile.prototype.Sprite_initialize = Mobile.prototype.initialize;

Mobile.prototype.initialize = function(serial, body, image, position, direction) {
  var spite, self = this;

  self.serial = serial;
  self.body = body;

  self.moving = false;
  self.idle = true;
  self.last_move = 0;

  self.location = { X: position.x, Y: position.y };
  self.last_location = { X: position.x, Y: position.y };
  self.last_direction = self.direction = direction;

  var pos = (Math.floor(body / 16) * 128) + ((body % 16) * 2);
  sprite = new createjs.SpriteSheet({
    images: [image],
    frames: { width: 32, height: 32, regX: 16, regY: 16 },
    animations: {
      north: [pos, pos + 1, 'north', 0.1],
      east: [pos + 32, pos + 33, 'east', 0.1],
      south: [pos + 64, pos + 65, 'south', 0.1],
      west: [pos + 96, pos + 97, 'west', 0.1],
      northeast: [pos, pos + 1, 'northeast', 0.1],
      northwest: [pos, pos + 1, 'northwest', 0.1],
      southeast: [pos + 64, pos + 65, 'southeast', 0.1],
      southwest: [pos + 64, pos + 65, 'southwest', 0.1]
    }
  });

  self.Sprite_initialize(sprite);

  self.currentFrame = 0;
  self.x = position.x;
  self.y = position.y;

  self.gotoAndStop('south');

  self.isMoving = function() {
    return Date.now() - self.last_move < 100;//TODO : conf
  };

  self.move = function(x, y, direction, force) {
    self.last_location.X = force ? x : self.location.X;
    self.last_location.Y = force ? y : self.location.Y;
    self.last_direction = force ? direction : self.direction;
    self.location.X = x;
    self.location.Y = y;
    self.direction = direction;
    self.last_move = Date.now();
  };
};
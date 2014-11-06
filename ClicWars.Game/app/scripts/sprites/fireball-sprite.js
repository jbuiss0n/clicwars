'use strict';

angular
  .module('clicwars.game')
  .factory('FireballSprite', [function() {
    var Fireball = function(serial, image, position, direction) {
      this.initialize(serial, image, position, direction);
    };
    Fireball.prototype = new createjs.Sprite();
    Fireball.prototype.Sprite_initialize = Fireball.prototype.initialize;
    Fireball.prototype.initialize = function(serial, image, position, direction) {
      var sprite, self = this;

      self.serial = serial;

      self.moving = false;
      self.idle = true;
      self.last_move = 0;

      self.location = { X: position.x, Y: position.y };
      self.last_location = { X: position.x, Y: position.y };
      self.last_direction = self.direction = direction;

      sprite = new createjs.SpriteSheet({
        images: [image],
        frames: { width: 32, height: 32, regX: 16, regY: 16 },
        animations: {
          north: [16, 23, 'north', 2],
          east: [32, 39, 'east', 2],
          south: [48, 55, 'south', 2],
          west: [0, 7, 'west', 2],
          northeast: [24, 31, 'northeast', 2],
          northwest: [8, 15, 'northwest', 2],
          southeast: [40, 47, 'southeast', 2],
          southwest: [56, 64, 'southwest', 2]
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

    return Fireball;
  }]);
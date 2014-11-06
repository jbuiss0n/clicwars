'use strict';

angular
  .module('clicwars.game')
  .factory('ExplosionSprite', [function() {
    var Explosion = function(image, position, onAnimationEnd) {
      this.initialize(image, position, onAnimationEnd);
    };
    Explosion.prototype = new createjs.Sprite();
    Explosion.prototype.Sprite_initialize = Explosion.prototype.initialize;
    Explosion.prototype.initialize = function(image, position, onAnimationEnd) {
      var sprite, self = this;

      self.location = { X: position.x, Y: position.y };

      sprite = new createjs.SpriteSheet({
        images: [image],
        frames: { width: 32, height: 32, regX: 16, regY: 16 },
        animations: {
          play: [0, 40, false, 2]
        }
      });

      self.Sprite_initialize(sprite);

      sprite.addEventListener('animationend ', function() {
        onAnimationEnd(this);
      });

      self.currentFrame = 0;
      self.x = position.x;
      self.y = position.y;

      self.gotoAndPlay('play');
    };

    return Explosion;
  }]);

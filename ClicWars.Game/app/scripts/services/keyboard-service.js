'use strict';

angular
  .module('clicwars.game')
  .service('KeyboardService', [function() {
    var self = this;

    self.Keys = { Up: 38, Down: 40, Left: 37, Right: 39 };
    self.ArrowKeys = [self.Keys.Up, self.Keys.Down, self.Keys.Left, self.Keys.Right];

    self.pressedKeys = [];

    self.init = function() {
      $(document).keydown(function(e) {
        if (!self.pressedKeys.contains(e.keyCode)) {
          self.pressedKeys.push(e.keyCode);
        }
      });

      $(document).keyup(function(e) {
        if (self.pressedKeys.contains(e.keyCode)) {
          self.pressedKeys.del(e.keyCode);
        }
      });
    };

    self.isPress = function(keyCode) {
      return self.pressedKeys.contains(keyCode);
    };

    self.lastPress = function(arg) {
      if ($.isArray(arg)) {
        for (var i = self.pressedKeys.length - 1; i >= 0; i--) {
          if (arg.contains(self.pressedKeys[i])) {
            return self.pressedKeys[i];
          }
        }
      }
      else {
        for (var i = self.pressedKeys.length - 1; i >= 0; i--) {
          if (self.pressedKeys[i] == arg) {
            return self.pressedKeys[i];
          }
        }
      }
      return -1;
    };
  }]);
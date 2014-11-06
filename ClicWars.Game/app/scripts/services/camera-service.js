'use strict';

angular
  .module('clicwars.game')
  .service('CameraService', [function() {
    var self = this;

    self.X = 0;
    self.Y = 0;
    self.Width = 0;
    self.Height = 0;
    self.MapWidth = 0;
    self.MapHeight = 0

    self.setSize = function(width, height, mapWidth, mapHeight) {
      self.Width = width;
      self.Height = height;
      self.MapWidth = mapWidth;
      self.MapHeight = mapHeight;
    };

    self.positionOn = function(mobile) {
      mobile.x = self.Width / 2;
      mobile.y = self.Height / 2;
      self.X = mobile.location.X - mobile.x;
      self.Y = mobile.location.Y - mobile.y;

      if (self.X < 0) {
        mobile.x += self.X;
        self.X = 0;
      }
      if (self.Y < 0) {
        mobile.y += self.Y;
        self.Y = 0;
      }
      if (self.X + self.Width > self.MapWidth) {
        mobile.x += (self.X + self.Width) - self.MapWidth;
        self.X = self.MapWidth - self.Width;
      }
      if (self.Y + self.Height > self.MapHeight) {
        mobile.y += (self.Y + self.Height) - self.MapHeight;
        self.Y = self.MapHeight + self.Height;
      }
    };
  }]);
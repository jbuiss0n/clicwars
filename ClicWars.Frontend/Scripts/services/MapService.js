var app = angular.module('app');

app.service('MapService', ['$resource', 'SocketService', 'ContentService', 'CameraService', function ($resource, SocketService, ContentService, CameraService) {

  var canvas, stage, layers, tileset, width, height, self = this;

  var MapResource = $resource(CONFIG.BASE_API_URL + '/map/:id');

  var onLoad = function (packet) {
    var id = packet.MapId;

    MapResource.get({ id: id }, function (map) {
      layers = map.Layers;

      if (!$.isArray(layers) || layers.length < 1) {
        throw 'No layers found in map id: ' + id;
      }

      height = layers[0].length;
      width = layers[0][0].length;

      CameraService.setSize(canvas.width, canvas.height, width * CONFIG.TILE_SIZE, height * CONFIG.TILE_SIZE);

      tileset = new createjs.SpriteSheet({
        images: [ContentService.getImage('tileset').url],
        frames: { width: CONFIG.TILE_SIZE, height: CONFIG.TILE_SIZE, regX: 0, regY: 0, count: 120 },
        animations: {
          all: [0, 119]
        }
      });
    });
  };

  self.init = function (id) {
    SocketService.bind(PACKET.MAP_EVENT.LOAD, onLoad);

    canvas = document.getElementById(id);
    stage = new createjs.Stage(canvas);
  };
  
  self.reset = function () {
    stage.removeAllChildren();
  };

  self.canMove = function (x, y, direction) {
    return x > 0 && x < width * CONFIG.TILE_SIZE && y > 0 && y < height * CONFIG.TILE_SIZE;
  };

  self.update = function () {

    stage.removeAllChildren();

    if (!layers || !layers.length)
      return;

    var firstX = Math.floor(CameraService.X / CONFIG.TILE_SIZE);
    var firstY = Math.floor(CameraService.Y / CONFIG.TILE_SIZE);
    var offsetX = CameraService.X % CONFIG.TILE_SIZE;
    var offsetY = CameraService.Y % CONFIG.TILE_SIZE;

    for (var i = 0; i < layers.length; i++) {
      for (var x = 0; x < Math.ceil(CameraService.Width / CONFIG.TILE_SIZE) + 1; x++) {
        for (var y = 0; y < Math.ceil(CameraService.Height / CONFIG.TILE_SIZE) + 1; y++) {

          var tileId = layers[i][firstY + y][firstX + x];

          if (tileId == -1)
            continue;

          var tile = new createjs.Sprite(tileset);

          tile.x = (x * CONFIG.TILE_SIZE) - offsetX;
          tile.y = (y * CONFIG.TILE_SIZE) - offsetY;

          tile.gotoAndStop("all");
          tile.gotoAndStop(tileId);

          stage.addChild(tile);
        }
      }
    }
    stage.update();
  };

}]);
var app = angular.module('app');

app.service('WorldService', [
    'SocketService'
  , 'ContentService'
  , 'CameraService'
  , function (SocketService, ContentService, CameraService) {
    var canvas, stage, effects = [], self = this;

    self.EFFECT = {
      EXPLOSION: 0x0001,
    };

    self.init = function (id) {
      canvas = document.getElementById(id);
      stage = new createjs.Stage(canvas);

      stage.addEventListener("stagemousedown", onClick);

      SocketService.bind(PACKET.WORLD_EVENT.EFFECT, addEffect);
    };

    self.reset = function () {
      stage.removeAllChildren();
      effects = [];
    };

    self.update = function () {

      for (var i = 0; i < effects.length; i++) {
        updateEffect(effects[i]);
      }

      stage.update();
    };

    var addEffect = function (packet) {
      var effect;
      if (packet.Effect === self.EFFECT.EXPLOSION) {
        effect = new Explosion(ContentService.getImage('explosion').url, { x: packet.X, y: packet.Y });
      }
      setTimeout(removeEffect.bind(this, effect), 2000);
      effects.push(effect);
      stage.addChild(effect);
    };

    var removeEffect = function (effect) {
      stage.removeChild(effect);
    };

    var updateEffect = function (effect) {
      effect.x = effect.location.X - CameraService.X;
      effect.y = effect.location.Y - CameraService.Y;
    };

    var onClick = function (event) {
      SocketService.send(new FireballRequestPacket(event.rawX + CameraService.X, event.rawY + CameraService.Y));
    };
  }]);
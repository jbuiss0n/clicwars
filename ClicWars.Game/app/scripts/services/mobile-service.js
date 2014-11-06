'use strict';

angular
  .module('clicwars.game')
  .service('MobileService', [
    'DIRECTION',
    'PACKET',
    'MobilePacket',
    'SocketService',
    'ContentService',
    'KeyboardService',
    'CameraService',
    'FireballSprite',
    'MobileSprite',
    function(DIRECTION, PACKET, MobilePacket, SocketService, ContentService, KeyboardService, CameraService, FireballSprite, MobileSprite) {
      var canvas, stage, playerSerial, mobiles = [], self = this;

      self.init = function(id) {
        canvas = document.getElementById(id);
        stage = new createjs.Stage(canvas);

        SocketService.bind(PACKET.MOBILE_EVENT.STATUS, onLoginConfirm);
        SocketService.bind(PACKET.MOBILE_EVENT.INCOMING, onMobileIncomming);
        SocketService.bind(PACKET.MOBILE_EVENT.REMOVING, onMobileRemoving);
        SocketService.bind(PACKET.MOBILE_EVENT.MOVING, onMobileMoving);
        SocketService.bind(PACKET.MOBILE_EVENT.MOVE_REJECT, onMoveReject);
      };

      self.reset = function() {
        stage.removeAllChildren();
        mobiles = [];
      };

      self.update = function() {
        updatePlayer();

        for (var i = 0; i < mobiles.length; i++) {
          updateMobile(mobiles[i]);
        }

        stage.update();
      };

      var updateMobile = function(mobile) {
        mobile.x = mobile.location.X - CameraService.X;
        mobile.y = mobile.location.Y - CameraService.Y;

        if (mobile.isMoving() && mobile.moving != mobile.direction) {
          mobile.gotoAndPlay(DIRECTION.Value[mobile.direction]);
          mobile.moving = mobile.direction;
          mobile.idle = false;
        }
        else if (!mobile.isMoving() && !mobile.idle) {
          mobile.gotoAndStop(DIRECTION.Value[mobile.direction]);
          mobile.moving = false;
          mobile.idle = true;
        }
      };

      var updatePlayer = function() {
        var player = mobiles.first('serial', playerSerial);
        if (player === null)
          return;

        var keyPressed = KeyboardService.lastPress(KeyboardService.ArrowKeys);

        if (player !== null && keyPressed !== -1) {

          player.direction = 0;

          if (KeyboardService.isPress(KeyboardService.Keys.Right)) {
            player.direction += DIRECTION.East;
          }
          if (KeyboardService.isPress(KeyboardService.Keys.Left)) {
            player.direction += DIRECTION.West;
          }
          if (KeyboardService.isPress(KeyboardService.Keys.Up)) {
            player.direction += DIRECTION.North;
          }
          if (KeyboardService.isPress(KeyboardService.Keys.Down)) {
            player.direction += DIRECTION.South;
          }

          if (player.direction > 0) {
            SocketService.send(new MobilePacket.MovementRequestPacket(player.direction));
          }
        }
        CameraService.positionOn(player);
      };

      var onMobileIncomming = function(packet) {
        var mobile = mobiles.first('serial', packet.Serial);
        if (mobile == null) {

          var position = { x: Math.round(packet.X), y: Math.round(packet.Y) };

          if (packet.Body < 0) {
            mobile = new FireballSprite(packet.Serial, ContentService.getImage('fireball').url, position, packet.Direction);
          }
          else {
            mobile = new MobileSprite(packet.Serial, packet.Body, ContentService.getImage('mobiles').url, position, packet.Direction);
          }
          mobiles.push(mobile);
          stage.addChild(mobile);
        }
      };

      var onMobileMoving = function(packet) {
        var mobile = mobiles.first('serial', packet.Serial);
        if (mobile !== null) {
          mobile.move(Math.round(packet.X), Math.round(packet.Y), packet.Direction);
        }
      };

      var onMobileRemoving = function(packet) {
        var mobile = mobiles.first('serial', packet.Serial);
        if (mobile !== null) {
          mobiles.del(mobile);
          stage.removeChild(mobile);
        }
      };

      var onMoveReject = function(packet) {
        var player = mobiles.first('serial', playerSerial);
        if (player !== null) {
          player.move(Math.round(packet.X), Math.round(packet.Y), packet.Direction, true);
        }
      };

      var onLoginConfirm = function(packet) {
        playerSerial = packet.Serial;
      }
    }]);
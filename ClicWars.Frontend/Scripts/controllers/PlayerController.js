var app = angular.module('app');

app.controller('PlayerController', [
  '$rootScope'
  , '$scope'
  , 'SocketService'
  , 'KeyboardService'
  , function ($rootScope, $scope, SocketService, KeyboardService) {
    var serial = $rootScope.serial, self = this;

    var update = function () {
      var keyPressed = KeyboardService.lastPress(KeyboardService.ArrowKeys);

      if (keyPressed != -1) {

        var direction = 0;

        if (KeyboardService.isPress(KeyboardService.Keys.Right)) {
          direction += Direction.East;
        }
        if (KeyboardService.isPress(KeyboardService.Keys.Left)) {
          direction += Direction.West;
        }
        if (KeyboardService.isPress(KeyboardService.Keys.Up)) {
          direction += Direction.North;
        }
        if (KeyboardService.isPress(KeyboardService.Keys.Down)) {
          direction += Direction.South;
        }

        if (direction > 0) {
          SocketService.send(new MovementRequestPacket(direction));
        }
      }
    };

    var onStatus = function (packet) {
      $scope.bodyBackgroundPosition = getBodyImage(packet.body);

      $scope.hits = packet.Hits;
      $scope.maxHits = packet.MaxHits;
      $scope.mana = packet.Mana;
      $scope.maxMana = packet.MaxMana;

      $scope.caracteristics = [
        { name: 'Speed', value: packet.Speed },
        { name: 'Hits per second', value: packet.RegenHits },
        { name: 'Mana per second', value: packet.RegenMana },
        //{ name: 'Fireball speed', value: packet.RegenMana },
        //{ name: 'Fireball range', value: packet.RegenHits },
      ];
      $scope.$apply();
    };

    var getBodyImage = function (body) {
      var x = (body % 16) * -64;
      var y = (Math.floor(body / 16) * -128) - 64;
      return x + 'px ' + y + 'px';
    };

    SocketService.bind(PACKET.MOBILE_EVENT.STATUS, onStatus);
    createjs.Ticker.addEventListener('tick', function () {
      if ($rootScope.status == $rootScope.STATUS.ALIVE) {
        update();
      }
    });
  }]);
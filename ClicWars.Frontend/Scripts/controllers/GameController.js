'use strict';

var app = angular.module('app');

app.controller('GameController', [
  '$rootScope'
  , '$scope'
  , '$location'
  , '$window'
  , 'GameTimeService'
  , 'ContentService'
  , 'SocketService'
  , 'KeyboardService'
  , 'WorldService'
  , 'MapService'
  , 'MobileService'
  , function($rootScope, $scope, $location, $window, GameTimeService, ContentService, SocketService, KeyboardService, WorldService, MapService, MobileService) {
    var self = this;

    if (!$rootScope.username || !$rootScope.serial) {
      $location.path("/create");
    }

    var onLogin = function() {
      SocketService.send(new LoginPacket($rootScope.username, $rootScope.serial));

      createjs.Ticker.setFPS(45);
      createjs.Ticker.addEventListener('tick', function() {
        if ($scope.status == $scope.STATUS.ALIVE) {
          MobileService.update();
          MapService.update();
          WorldService.update();
          GameTimeService.update();
        }
      });
    };

    var onLoad = function(packet) {
      MobileService.reset();
      MapService.reset();
      WorldService.reset();
      $rootScope.status = $rootScope.STATUS.ALIVE;
      $('#' + CONFIG.CANVAS.MAP + ', #' + CONFIG.CANVAS.MOBILE + ', #' + CONFIG.CANVAS.WORLD).fadeIn(500);
    };

    var onDeath = function() {
      $('#' + CONFIG.CANVAS.MAP + ', #' + CONFIG.CANVAS.MOBILE + ', #' + CONFIG.CANVAS.WORLD).fadeOut(1500, function() {
        $scope.$apply(function() {
          $scope.status = $rootScope.status = $rootScope.STATUS.DEAD;
        });
      });
    };

    var onDestroy = function() {
      createjs.Ticker.removeAllEventListeners();
      SocketService.close();
    };

    $scope.STATUS = $rootScope.STATUS = {
      ALIVE: 0x01,
      DEAD: 0x02
    };

    $rootScope.status = $scope.STATUS.ALIVE;

    ContentService.init(function () {
      SocketService.bind(SocketService.EVENT.CONNECTED, onLogin);
      SocketService.bind(PACKET.MAP_EVENT.LOAD, onLoad);
      SocketService.bind(PACKET.PLAYER_EVENT.DEATH, onDeath);

      KeyboardService.init();
      WorldService.init(CONFIG.CANVAS.WORLD);
      MapService.init(CONFIG.CANVAS.MAP);
      MobileService.init(CONFIG.CANVAS.MOBILE, $rootScope.serial);

      SocketService.start(CONFIG.SERVER_LOCATION);
    });

    $scope.respawn = function () {
      //SocketService.send(new PlayerRespawnRequestPacket($rootScope.serial));
      $window.location.reload();
    };

    $scope.$on('$destroy', onDestroy);
  }]);
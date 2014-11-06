﻿'use strict';

angular
  .module('clicwars.game')
  .controller('GameController', [
    '$scope',
    '$cookieStore',
    'PACKET',
    'GamePacket',
    'ContentService',
    'SocketService',
    'KeyboardService',
    'WorldService',
    'MapService',
    'MobileService',
    function($scope, $cookieStore, PACKET, GamePacket, ContentService, SocketService, KeyboardService, WorldService, MapService, MobileService) {
      var self = this, _isLoad = false;

      var gameToken = $cookieStore.get('game-token')

      if (!gameToken)
        return;

      var onConnected = function() {
        SocketService.send(new GamePacket.LoginPacket(gameToken.username, gameToken.token));

        createjs.Ticker.setFPS(45);
        createjs.Ticker.addEventListener('tick', function() {
          if (_isLoad) {
            MobileService.update();
            MapService.update();
            WorldService.update();
          }
        });
      };

      var onLoad = function(packet) {
        MobileService.reset();
        MapService.reset();
        WorldService.reset();
        _isLoad = true;
      };

      var onDestroy = function() {
        createjs.Ticker.removeAllEventListeners();
        SocketService.close();
      };

      $scope.respawn = function() {
        //SocketService.send(new PlayerRespawnRequestPacket($rootScope.serial));
        //$window.location.reload();
      };

      $scope.$on('$destroy', onDestroy);

      ContentService.init(function() {

        SocketService.bind(SocketService.EVENT.CONNECTED, onConnected);
        SocketService.bind(PACKET.MAP_EVENT.LOAD, onLoad);

        KeyboardService.init();
        WorldService.init(CONFIG.CANVAS.WORLD);
        MapService.init(CONFIG.CANVAS.MAP);
        MobileService.init(CONFIG.CANVAS.MOBILE);

        SocketService.start(CONFIG.SERVER_LOCATION);
      });
    }]);
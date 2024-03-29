﻿'use strict';

angular
  .module('clicwars.game')
  .controller('GameController', [
    '$scope',
    'ipCookie',
    'PACKET',
    'GamePacket',
    'ContentService',
    'SocketService',
    'KeyboardService',
    'WorldService',
    'MapService',
    function($scope, ipCookie, PACKET, GamePacket, ContentService, SocketService, KeyboardService, WorldService, MapService) {
      var self = this, _isConnected = false;

      var gameToken = ipCookie('game-token');

      if (!gameToken) {
        window.location = CONFIG.BASE_ACCOUNT_URL;
        return;
      }

      var onConnected = function() {
        SocketService.send(new GamePacket.LoginPacket(gameToken.username, gameToken.token));

        createjs.Ticker.setFPS(45);
        createjs.Ticker.addEventListener('tick', function() {
          if (_isConnected) {
            MapService.update();
            WorldService.update();
          }
        });
      };

      var onDisconnected = function() {
        _isConnected = false;
      };

      var onLoad = function(packet) {
        MapService.reset();
        WorldService.reset();
        _isConnected = true;
      };

      var onDestroy = function() {
        _isConnected = false;
        createjs.Ticker.removeAllEventListeners();
        SocketService.close();
      };

      ContentService.init(function() {

        SocketService.bind(SocketService.EVENT.CONNECTED, onConnected);
        SocketService.bind(SocketService.EVENT.DISCONNECTED, onDisconnected);
        SocketService.bind(PACKET.MAP_EVENT.LOAD, onLoad);

        KeyboardService.init();
        WorldService.init(CONFIG.CANVAS.WORLD);
        MapService.init(CONFIG.CANVAS.MAP);

        SocketService.start(CONFIG.SERVER_LOCATION);
      });

      $scope.$on('$destroy', onDestroy);
    }]);
'use strict';

var app = angular.module('app');

app.controller('ChatController', [
  '$rootScope'
  , '$scope'
  , 'SocketService'
  , function ($rootScope, $scope, SocketService) {
    var self = this;

    var CHAT_TYPE = {
      WORLD: 1,
      MAP: 2,
      PRIVATE: 3,
    };

    $scope.messages = [];

    $scope.send = function () {
      SocketService.send(new ChatMessagePacket(CHAT_TYPE.MAP, $rootScope.serial, $scope.message));
      $scope.message = '';
    };

    var onChatMessage = function (packet) {
      $scope.messages.push(packet.Message);
      $scope.$apply();
    };

    SocketService.bind(PACKET.WORLD_EVENT.CHAT, onChatMessage);
  }]);
'use strict';

var app = angular.module('app');

app.service('SocketService', ['$rootScope', function ($rootScope) {
  var ws, handlers = [], self = this;

  self.EVENT = {
    CONNECTED: 'GAME_CONNECTED',
    DISCONNECTED: 'GAME_DISCONNECTED',
    ERROR: 'GAME_ERROR',
    MESSAGE: 'GAME_MESSAGE',
  };

  self.start = function (location) {
    ws = new WebSocket(location);
    ws.onopen = onSocketOpen;
    ws.onmessage = onSocketMessage;
    ws.onclose = onSocketClose;
    ws.onerror = onSocketError;
  };

  self.send = function (packet) {
    var json = toZeroBasedString(packet.Id) + JSON.stringify(packet);
    ws.send(json);
  };

  self.bind = function (trigger, callback) {
    handlers[trigger] = handlers[trigger] || [];
    handlers[trigger].push(callback);
  };

  self.close = function () {
    handlers = [];
    ws.close();
  };

  var raiseEvent = function (trigger, packet) {
    if (handlers[trigger]) {
      handlers[trigger].forEach(function (handler) {
        handler(packet);
      });
    }
  };

  var onSocketOpen = function () {
    raiseEvent(self.EVENT.CONNECTED);
  };

  var onSocketClose = function () {
    raiseEvent(self.EVENT.DISCONNECTED);
  };

  var onSocketError = function () {
    raiseEvent(self.EVENT.ERROR);
  };

  var onSocketMessage = function (event) {
    raiseEvent(self.EVENT.MESSAGE, event);

    var packet = JSON.parse(event.data.substring(4));

    raiseEvent(packet.Id, packet);
  };
}]);
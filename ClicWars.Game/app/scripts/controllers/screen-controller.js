angular
  .module('clicwars.game')
  .controller('ScreenController', [
    '$scope',
    'PACKET',
    'SocketService',
    function($scope, PACKET, SocketService) {
      var self = this;

      var onDeath = function(packet) {

      };

      var getBodyPosition = function(body) {
        var x = (body % 16) * -64;
        var y = (Math.floor(body / 16) * -128) - 64;
        return x + 'px ' + y + 'px';
      };

      SocketService.bind(PACKET.PLAYER_EVENT.DEATH, onDeath);
    }]);
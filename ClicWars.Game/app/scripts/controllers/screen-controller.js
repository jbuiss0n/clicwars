angular
  .module('clicwars.game')
  .controller('ScreenController', [
    '$scope',
    'PACKET',
    'PlayerPacket',
    'SocketService',
    'WorldService',
    function($scope, PACKET, PlayerPacket, SocketService, WorldService) {
      var self = this;

      var onDeath = function(packet) {
        $scope.isDead = true;
        $scope.killer = WorldService.find(packet.Killer);
      };

      self.bodyBackgroundPosition = function(body) {
        var x = (body % 16) * -64;
        var y = (Math.floor(body / 16) * -128) - 64;
        return x + 'px ' + y + 'px';
      };

      self.onRespawn = function() {
        SocketService.send(new PlayerPacket.RespawnRequestPacket());
        $scope.isDead = false;
        $scope.killer = false;
      };

      SocketService.bind(PACKET.PLAYER_EVENT.DEATH, onDeath);
      SocketService.bind(PACKET.PLAYER_EVENT.DEATH, onDeath);
    }]);
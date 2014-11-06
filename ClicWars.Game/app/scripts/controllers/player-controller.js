angular
  .module('clicwars.game')
  .controller('PlayerController', [
    '$scope',
    'PACKET',
    'SocketService',
    'KeyboardService',
    function($scope, PACKET, SocketService, KeyboardService) {
      var self = this;

      var onStatus = function(packet) {
        $scope.bodyBackgroundPosition = getBodyPosition(packet.Body);

        $scope.hits = packet.Hits;
        $scope.maxHits = packet.HitsMax;
        $scope.mana = packet.Mana;
        $scope.maxMana = packet.ManaMax;

        $scope.caracteristics = [
          { name: 'Speed', value: packet.Speed },
          { name: 'Hits per second', value: packet.RegenHits },
          { name: 'Mana per second', value: packet.RegenMana }
        ];
        $scope.$apply();
      };

      var getBodyPosition = function(body) {
        var x = (body % 16) * -64;
        var y = (Math.floor(body / 16) * -128) - 64;
        return x + 'px ' + y + 'px';
      };

      SocketService.bind(PACKET.PLAYER_EVENT.STATUS, onStatus);
    }]);
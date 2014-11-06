'use strict';

angular
  .module('clicwars.game')
  .factory('PageService', [function () {
    var _title = 'Clic Wars :: Game';

    return {
      title: function (value) {
        if (value) {
          _title = value;
        }
        else {
          return _title;
        }
      }
    };
  }]);
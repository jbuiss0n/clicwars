'use strict';

angular
  .module('clicwars.account')
  .factory('PageService', [
    function() {
      var _title = 'Clic Wars :: Account';

      return {
        title: function(value) {
          if (value) {
            _title = value;
          }
          else {
            return _title;
          }
        }
      };
    }]);
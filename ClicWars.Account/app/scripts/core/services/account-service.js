'use strict';

angular
  .module('clicwars.account')
  .factory('AccountService', [
    '$resource',
    function($resource) {
      return $resource(CONFIG.BASE_API_URL + '/Accounts', {}, {
        create: {
          method: 'POST'
        },
        login: {
          method: 'Get'
        }
      });
    }]);

'use strict';

angular
  .module('clicwars.account')
  .factory('AuthService', [
    '$resource',
    'ipCookie',
    function($resource, ipCookie) {
      var _cookie = ipCookie('auth') || {};

      var updateCookie = function() {
        ipCookie('auth', _cookie, { path: '/' });
      };

      return {
        authenticate: function(data) {
          this.token(data.Token);
          this.username(data.Username);
          this.expires(data.Expires);
        },
        isAuthenticated: function() {
          return Date.now() < this.expires()
            && !!this.token()
            && !!this.token();
        },
        token: function(value) {
          if (value) {
            _cookie.token = value;
            updateCookie();
          }
          else {
            return _cookie.token || '';
          }
        },
        username: function(value) {
          if (value) {
            _cookie.username = value;
            updateCookie();
          }
          else {
            return _cookie.username || '';
          }
        },
        expires: function(value) {
          if (value) {
            _cookie.expires = value;
            updateCookie();
          }
          else {
            return _cookie.expires || 0;
          }
        }
      };
    }]);

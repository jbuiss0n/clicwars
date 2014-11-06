'use strict';

angular
  .module('clicwars.account')
  .factory('AuthService', [
    '$resource',
    '$cookies',
    function($resource, $cookies) {
      return {
        authenticate: function(data) {
          this.token(data.Token);
          this.username(data.Username);
          $cookies.expires = data.Expires;
        },
        isAuthenticated: function() {
          return Date.now() < this.expires()
            && !!this.token()
            && !!this.token();
        },
        token: function(value) {
          if (value) {
            $cookies.token = value;
          }
          else {
            return $cookies.token || '';
          }
        },
        username: function(value) {
          if (value) {
            $cookies.username = value;
          }
          else {
            return $cookies.username || '';
          }
        },
        expires: function(value) {
          if (value) {
            $cookies.expires = value;
          }
          else {
            return $cookies.expires || 0;
          }
        }
      };
    }]);

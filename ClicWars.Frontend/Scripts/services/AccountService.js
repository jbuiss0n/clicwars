'use strict';

var app = angular.module('app');

app.service('AccountService', ['$resource', function ($resource) {
  return $resource(CONFIG.BASE_API_URL + '/Account');
}]);
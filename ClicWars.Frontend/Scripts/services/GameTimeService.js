var app = angular.module('app');

app.service('GameTimeService', [function () {
  var self = this;
  var time;
  var lastUpdate;

  this.update = function () {
    var now = new Date().getTime();

    self.total = now - time;
    self.elapsed = lastUpdate - now;

    lastUpdate = now;
  };

  this.reset = function () {
    time = new Date().getTime();
    lastUpdate = time;

    self.total = 0;
    self.elapsed = 0;
  };

  self.reset();
}]);
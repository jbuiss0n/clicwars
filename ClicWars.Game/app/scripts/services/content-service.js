'use strict';

angular
  .module('clicwars.game')
  .service('ContentService', [function() {
    var imageAssets = [], elementsLoaded = 0, complete, self = this;

    self.init = function(onComplete) {

      complete = onComplete;

      imageAssets = [
        { name: 'mobiles', url: 'images/sprites/characters.png' },
        { name: 'fireball', url: 'images/sprites/fireball.png' },
        { name: 'explosion', url: 'images/sprites/explosion.png' },
        { name: 'tileset', url: 'images/tiles/basic-terrain.png' }
      ];

      if (elementsLoaded === imageAssets.length)
        return complete();

      for (var i = 0; i < imageAssets.length; i++) {
        downloadImage(imageAssets[i]);
      }
    };

    self.getImage = function(name) {
      return imageAssets.first('name', name);
    };

    var downloadImage = function(element) {
      element.asset = new Image();
      element.asset.onload = handleElementLoad;
      element.asset.src = element.url;
    };

    var handleElementLoad = function(e) {
      elementsLoaded++;
      if (elementsLoaded === imageAssets.length) {
        complete();
      }
    };
  }]);
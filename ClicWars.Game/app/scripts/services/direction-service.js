'use strict';

angular
  .module('clicwars.game')
  .value('DIRECTION', {
    North: 1,
    East: 2,
    South: 4,
    West: 8,
    Value: {
      1: 'north',
      2: 'east',
      4: 'south',
      8: 'west',
      
      3: 'northeast',
      9: 'northwest',
      6: 'southeast',
      12: 'southwest'
    }
  });
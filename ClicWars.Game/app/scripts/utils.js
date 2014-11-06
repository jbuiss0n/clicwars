/* Array utils */
Array.prototype.indexOf = function (o /*, index*/) {
  var index = Number(arguments[1]) || 0;

  index = (index < 0) ? Math.ceil(index) : Math.floor(index);

  if (index < 0)
    index += this.length;

  for (; index < this.length; index++) {
    if (index in this && this[index] === o)
      return index;
  }
  return -1;
};
Array.prototype.lastIndexOf = function (o) {
  var index = this.length;

  for (; index >= 0; index--) {
    if (index in this && this[index] === o)
      return index;
  }
  return -1;
};
Array.prototype.insertAt = function (o, index) {
  if (index > -1 && index <= this.length) {
    this.splice(index, 0, o);
    return true;
  }
  return false;
};
Array.prototype.insertBefore = function (o, toInsert) {
  var inserted = false;
  var index = this.indexOf(o);
  if (index == -1) {
    return false;
  }
  else {
    if (index == 0) {
      this.unshift(toInsert)
      return true;
    }
    else {
      return this.insertAt(toInsert, index - 1);
    }
  }
};
Array.prototype.insertAfter = function (o, toInsert) {
  var inserted = false;
  var index = this.indexOf(o);
  if (index == -1) {
    return false;
  }
  else {
    if (index == this.length - 1) {
      this.push(toInsert);
      return true;
    }
    else {
      return this.insertAt(toInsert, index + 1);
    }
  }
};
Array.prototype.remove = function (from, to) {
  var rest = this.slice((to || from) + 1 || this.length);
  this.length = from < 0 ? this.length + from : from;
  return this.push.apply(this, rest);
};
Array.prototype.first = function (attribut, value) {
  for (var i = 0; i < this.length; i++) {
    if (this[i][attribut] == value)
      return this.slice(i, i + 1)[0];
  }
  return null;
};
Array.prototype.where = function (attribut, value) {
  var res = [];
  for (var i = 0; i < this.length; i++) {
    if (this[i][attribut] == value)
      res.push(this.slice(i, i + 1));
  }
  return res;
};
Array.prototype.contains = function (o) {
  return this.indexOf(o) != -1;
};
Array.prototype.del = function (o) {
  var idx = this.indexOf(o);
  while (idx != -1) {
    this.remove(idx);
    idx = this.indexOf(o);
  }
};
String.prototype.replaceAll = function (replace, value) {
  return this.replace(new RegExp(replace, 'g'), value);
}

/* Number utils */
var toZeroBasedString = function (num) {
  var r = '' + num;
  while (r.length < 4) {
    r = '0' + r;
  }
  return r;
}
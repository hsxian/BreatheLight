function change() {
  //var bsLbl = document.getElementById("bsLbl");
  //var slider = document.getElementById("slider");
  //console.log(slider.value);
  //bsLbl.value = slider.value;
  SetLightBrightness();
}

function move() {
  var bsLbl = document.getElementById("bsLbl");
  var slider = document.getElementById("slider");
  bsLbl.value = slider.value;
}
function less() {
  var bsLbl = document.getElementById("bsLbl");
  var slider = document.getElementById("slider");
  // var value= parseInt()
  //console.log(--slider.value);
  if (parseInt(slider.value) > parseInt(slider.min)) {
    bsLbl.value = --slider.value;
    SetLightBrightness();
  }
}
function plus() {
  var bsLbl = document.getElementById("bsLbl");
  var slider = document.getElementById("slider");
  //console.log(++slider.value);
  if (parseInt(slider.value) < parseInt(slider.max)) {
    bsLbl.value = ++slider.value;
    SetLightBrightness();
  }
}

function SetLightBrightness() {
  var bsLbl = document.getElementById("bsLbl");
  var slider = document.getElementById("slider");
  slider.value = bsLbl.value;
  console.log(bsLbl.value);
  var delay = new Delayer(function s() {
    $.get("home/SetLightBrightness?brightness=" + slider.value);
  }, 300);
  delay.delay();
}

function Delayer(callback, delayTime) {
  this.callback = callback;
  this.count = 0;
  this.delayTime = delayTime;
}

Delayer.prototype.delay = function() {
  if (++this.count == 1) {
    var self = this;
    setTimeout(function() {
      try {
        self.callback();
      } catch (err) {
      } finally {
        //执行完后将值清空，保证下次还能执行
        self.count = 0;
      }
    }, this.delayTime);
  }
};

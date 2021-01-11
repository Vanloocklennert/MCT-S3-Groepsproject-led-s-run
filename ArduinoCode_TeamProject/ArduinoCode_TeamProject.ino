#include <Adafruit_NeoPixel.h>
Adafruit_NeoPixel strip = Adafruit_NeoPixel(43, 6);


void setup() {
  strip.begin();
  strip.setBrightness(50);
  strip.clear();
  strip.show();


  
}

void loop() {


    for(int x = 0;x<43;x++){
    strip.setPixelColor(x-1, 0,0,0);
    strip.setPixelColor(x , 255,0,0);
    strip.show();
    delay((0.005 * 1000));
  }
}

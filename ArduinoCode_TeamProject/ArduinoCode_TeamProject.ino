#include <Adafruit_NeoPixel.h>
Adafruit_NeoPixel strip = Adafruit_NeoPixel(43, 6);
const double distanceOneLed = 1.6; // cm  

void setup() {
  strip.begin();
  strip.setBrightness(50);
  strip.clear();
  strip.show();
  Serial.begin(9600);

  
}

void loop() {

    Serial.println(ledDelayMs, 8);
    for(int x = 0;x<43;x++){
    strip.setPixelColor(x-1, 0,0,0);
    strip.setPixelColor(x , 255,0,0);
    strip.show();
    delay((ledDelayMs));
}
}




float CalculateLedDelay(float velocity){
    float velocityMeters = (velocity/3.6) * 100;
    float ledDelay = (velocityMeters / distanceOneLed);
    float ledDelaySecond = 1 / ledDelay;
    float ledDelayMs = ledDelaySecond * 1000;

    
    return ledDelayMs; 
}








  

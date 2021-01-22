#include <Adafruit_NeoPixel.h>
Adafruit_NeoPixel strip = Adafruit_NeoPixel(43, 6);
const double distanceOneLed = 1.6; // cm  
int workoutData [10][2];

 float  beginTime1;
 float beginTime2;

void setup() {
  strip.begin();
  strip.setBrightness(50);
  strip.clear();
  strip.show();
  Serial.begin(9600);

  beginTime1 = millis();
  beginTime2 = millis(); 
 
}

void loop() {
 if (Serial.available() > 0) {
    String data = Serial.readStringUntil('#');

    Serial.println(data);
    String workoutVelocity = seperateString(data, '&', 0);
    String workoutTime = seperateString(data, '&', 1);

    Serial.println(workoutVelocity);

    for (int x = 0; x < 10; x++){
        if(workoutData[x] == ""){
            Serial.println(workoutVelocity);
            Serial.println(workoutTime);
            workoutData[x][0] =   workoutVelocity.toFloat();
            workoutData[x][1] = workoutTime.toFloat();
            break;
        }
    } 
 }




float elapsedTime1 = millis() - beginTime1;
float elapsedTime2 = millis() - beginTime2;

int elapsedLeds = round(elapsedTime1 / 26);
int elapsedLeds2 = round(elapsedTime2 / 25);


if (elapsedLeds > 42) beginTime1 = millis();
if (elapsedLeds2 > 42) beginTime2 = millis();

strip.setPixelColor(elapsedLeds - 1, 0,0,0);
strip.setPixelColor(elapsedLeds, 255,0,0);

strip.setPixelColor(elapsedLeds2 - 1, 0,0,0);
strip.setPixelColor(elapsedLeds2, 0,255,0);
strip.show();

}




float CalculateLedDelay(float velocity){
    float velocityMeters = (velocity/3.6) * 100;
    float ledDelay = (velocityMeters / distanceOneLed);
    float ledDelaySecond = 1 / ledDelay;
    float ledDelayMs = ledDelaySecond * 1000;

    
    return ledDelayMs; 
}


String seperateString(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = { 0, -1 };
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) {
        if (data.charAt(i) == separator || i == maxIndex) {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}








  

#include <FastLED.h>
#define NUM_LEDS 150
#define DATA_PIN 6
CRGB leds[NUM_LEDS];


const double distanceOneLed = 10.5; // cm  
float workoutData [10][4] = {NULL};
int workoutDataColors[10][3] = {NULL};
char red[3];
char green[3];
char blue[3];


 float  beginTime1;
 float beginTime2;

void setup() {
  FastLED.addLeds<NEOPIXEL, DATA_PIN>(leds, NUM_LEDS);
  FastLED.setBrightness(50);
  Serial1.begin(11800);
  Serial.begin(9600);
  Serial1.setTimeout(5000);

  beginTime1 = millis();
  beginTime2 = millis(); 

  FastLED.clear();  // clear all pixel data
  FastLED.show();
}

void loop() {

    digitalWrite(8, HIGH);
    digitalWrite(8, LOW);

    if (Serial1.available()){
    String data = Serial1.readStringUntil('%');

    Serial.println("Data: " + data);
    String workoutTime = seperateString(data, '&', 1);
    String workoutVelocity = seperateString(data, '&', 2);
    String workoutColor = seperateString(data, '&', 3);

    for (int x = 1; x < 10; x++){
        Serial.println("test");
        if(workoutData[x][0] == NULL){
            Serial.println(workoutVelocity);
            Serial.println(workoutTime);
            Serial.println(workoutColor);

            String redString = workoutColor.substring(0,2);
            String greenString = workoutColor.substring(2,4);
            String blueString = workoutColor.substring(4,6);

            redString.toCharArray(red, 3);
            greenString.toCharArray(green, 3);
            blueString.toCharArray(blue, 3);

            workoutData[x][0] = workoutVelocity.toFloat();
            workoutData[x][1] = millis() + workoutTime.toFloat();
            workoutData[x][2] = millis();
            workoutDataColors[x][0] = StrToHex(red);
            workoutDataColors[x][1] = StrToHex(green);
            workoutDataColors[x][2] = StrToHex(blue);
            Serial.println(workoutTime.toFloat());
            break;
        }
    }
 }


 for (int x = 1; x < 10; x++){
    if (!workoutData[x][0] == NULL) {       
    float elapsedTime = millis() - workoutData[x][2];
    int elapsedLeds = round(elapsedTime / CalculateLedDelay(workoutData[x][0]));

    if (elapsedLeds >= 150){
      workoutData[x][2] = millis(); 
    }

    if (millis() >= workoutData[x][1]){
        Serial.println(workoutData[x][1]);
        Serial.println(millis());
        workoutData[x][0] = NULL;
        FastLED.clear();
        FastLED.show();
        break;
    }


    leds[elapsedLeds].setRGB(workoutDataColors[x][0],workoutDataColors[x][1],workoutDataColors[x][2]);
    if (!Serial1.available()) FastLED.show();
    leds[elapsedLeds].setRGB(0,0,0); 
    }
  
 }

delay(5);
}




float CalculateLedDelay(float velocity){
    float velocityMeters = velocity * 100;
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


int StrToHex(char str[])
{
  return (int) strtol(str, NULL, 16);
}
  

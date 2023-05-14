#include "Adafruit_NeoPixel.h"

using namespace std;


const int lightsPin1 = 5;
const int lightsPin2 = 17;
const int lightsPin3 = 26;
const int lightsPin4 = 35;

// which pins are connected to buttons?

const int numButtons = 8;

const int buttonPins1[] = {13,12,11,10,9,8,7,6}; //{6,7,8,9,10,11,12,13};
const int buttonPins2[] = {4,3,2,36,37,14,15,16}; //{16,15,14,37,36,2,3,4};
const int buttonPins3[] = {18,19,20,21,22,23,24,25}; //{25,24,23,22,21,20,19,18};
const int buttonPins4[] = {34,33,32,31,30,29,28,27}; //{27,28,29,30,31,32,33,34};

String lastState = "0,0,0,0,0,0,0,0:0,0,0,0,0,0,0,0:0,0,0,0,0,0,0,0:0,0,0,0,0,0,0,0";
String lastLightInputString = "100000000";

Adafruit_NeoPixel pixels1(numButtons, lightsPin1, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels2(numButtons, lightsPin2, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels3(numButtons, lightsPin3, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels4(numButtons, lightsPin4, NEO_GRB + NEO_KHZ800);

const int buttonReds[] = {255, 255, 255, 119, 0, 0, 188, 255};
const int buttonGreens[] = {0, 154, 239, 255, 255, 60, 0, 0};
const int buttonBlues[] = {0, 0, 0, 0, 252, 255, 255, 0};

const float chordFactor = 0.6;

void setup() {

 // configure the digital input:
  for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
    pinMode(buttonPins1[pinNumber], INPUT);
    pinMode(buttonPins2[pinNumber], INPUT);
    pinMode(buttonPins3[pinNumber], INPUT);
    pinMode(buttonPins4[pinNumber], INPUT);

 }

  Serial.setTimeout(50);
  Serial.begin(9600); 
  pixels1.begin();
  pixels2.begin();
  pixels3.begin();
  pixels4.begin();

  testNeopixels();
}

// turn on all the connected neopixels
void testNeopixels() {
  for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels1.setPixelColor(i, pixels1.Color(255, 0, 0));
    pixels1.show();  

    pixels2.setPixelColor(i, pixels2.Color(255, 0, 0));
    pixels2.show();  

    pixels3.setPixelColor(i, pixels3.Color(255, 0, 0));
    pixels3.show();  

    pixels4.setPixelColor(i, pixels4.Color(255, 0, 0));
    pixels4.show();  
  }

  delay(1000);

  for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels1.setPixelColor(i, pixels1.Color(0, 255, 0));
    pixels1.show();  

    pixels2.setPixelColor(i, pixels2.Color(0, 255, 0));
    pixels2.show();  

    pixels3.setPixelColor(i, pixels3.Color(0, 255, 0));
    pixels3.show();  

    pixels4.setPixelColor(i, pixels4.Color(0, 255, 0));
    pixels4.show();  
  }
    delay(1000);

    for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels1.setPixelColor(i, pixels1.Color(0, 0, 255));
    pixels1.show();  
    pixels2.setPixelColor(i, pixels2.Color(0, 0, 255));
    pixels2.show();  
    pixels3.setPixelColor(i, pixels3.Color(0, 0, 255));
    pixels3.show();  
    pixels4.setPixelColor(i, pixels4.Color(0, 0, 255));
    pixels4.show();  
  }

      delay(1000);

    for(int i=0; i<numButtons; i++) { // For each pixel...
        pixels1.setPixelColor(i, pixels1.Color(255, 255, 255));
        pixels1.show();  

        pixels2.setPixelColor(i, pixels2.Color(255, 255, 255));
        pixels2.show(); 

        pixels3.setPixelColor(i, pixels3.Color(255, 255, 255));
        pixels3.show();  

        pixels4.setPixelColor(i, pixels4.Color(255, 255, 255));
        pixels4.show();  
        
    }

    delay(1000);

    pixels1.clear();
    pixels2.clear();
    pixels3.clear();
    pixels4.clear();

    pixels1.show();  
    pixels2.show();  
    pixels3.show();  
    pixels4.show();  


}


 // check buttons and write values to serial; returns true if they've changed
bool checkButtons() {
    String curr = "";
    for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins1[pinNumber]);
        curr += String(sensorValue);
        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          curr += ",";
        }
    }
    curr += ":";

       for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins2[pinNumber]);
        curr += String(sensorValue);
        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          curr += ",";
        }
    }

    curr += ":";

       for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins3[pinNumber]);
        curr += String(sensorValue);
        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          curr += ",";
        }
    }

    curr += ":";

       for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins4[pinNumber]);
        curr += String(sensorValue);
        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          curr += ",";
        }
    }


    if (!curr.equals(lastState)){
      Serial.println(curr);
      return true;
    }
    lastState = curr;
    return false;
}

 // check serial data for light info and update lights accordingly
void updateLights(bool buttonsChanged) {

    bool chordsChanged = false;
    if (Serial.available() >= numButtons + 2) {
      lastLightInputString = Serial.readStringUntil('/n');
      chordsChanged = true;
    }

    if(!(buttonsChanged || chordsChanged)) {
      return;
    }

    char targetController = lastLightInputString.charAt(0);

  
    pixels1.clear();
    pixels2.clear();
    pixels3.clear();
    pixels4.clear();
    for(int lightIndex = 0; lightIndex < lastLightInputString.length() - 1; lightIndex++) {
      bool lightInChord = lastLightInputString.charAt(lightIndex + 1) == '1';


      if(targetController == '1') {
        bool buttonPressed = digitalRead(buttonPins1[lightIndex]) == 1;
        if(lightInChord) pixels1.setPixelColor(lightIndex, pixels1.Color(buttonReds[lightIndex] * chordFactor, 
                                                                         buttonGreens[lightIndex] * chordFactor, 
                                                                         buttonBlues[lightIndex] * chordFactor));
        else pixels1.setPixelColor(lightIndex, pixels1.Color(5, 5, 5));

        if (buttonPressed) pixels1.setPixelColor(lightIndex, pixels1.Color(buttonReds[lightIndex], 
                                                                           buttonGreens[lightIndex], 
                                                                           buttonBlues[lightIndex]));
      }

      if(targetController == '2') {
        bool buttonPressed = digitalRead(buttonPins2[lightIndex]) == 1;
        if(lightInChord) pixels2.setPixelColor(lightIndex, pixels2.Color(buttonReds[lightIndex] * chordFactor, 
                                                                         buttonGreens[lightIndex] * chordFactor, 
                                                                         buttonBlues[lightIndex] * chordFactor));
        else pixels2.setPixelColor(lightIndex, pixels2.Color(5, 5, 5));

        if (buttonPressed) pixels2.setPixelColor(lightIndex, pixels2.Color(buttonReds[lightIndex], 
                                                                           buttonGreens[lightIndex], 
                                                                           buttonBlues[lightIndex]));
      }

      if(targetController == '3') {
        bool buttonPressed = digitalRead(buttonPins3[lightIndex]) == 1;
        if(lightInChord) pixels3.setPixelColor(lightIndex, pixels3.Color(buttonReds[lightIndex] * chordFactor, 
                                                                         buttonGreens[lightIndex] * chordFactor, 
                                                                         buttonBlues[lightIndex] * chordFactor));
        else pixels3.setPixelColor(lightIndex, pixels3.Color(5, 5, 5));

        if (buttonPressed) pixels3.setPixelColor(lightIndex, pixels3.Color(buttonReds[lightIndex], 
                                                                           buttonGreens[lightIndex], 
                                                                           buttonBlues[lightIndex]));
      }

        if(targetController == '4') {
        bool buttonPressed = digitalRead(buttonPins4[lightIndex]) == 1;
        if(lightInChord) pixels4.setPixelColor(lightIndex, pixels4.Color(buttonReds[lightIndex] * chordFactor, 
                                                                         buttonGreens[lightIndex] * chordFactor, 
                                                                         buttonBlues[lightIndex] * chordFactor));
        else pixels4.setPixelColor(lightIndex, pixels4.Color(5, 5, 5));

        if (buttonPressed) pixels4.setPixelColor(lightIndex, pixels4.Color(buttonReds[lightIndex], 
                                                                           buttonGreens[lightIndex], 
                                                                           buttonBlues[lightIndex]));
      }


      // if(lightInputString.charAt(lightIndex + 1) == '1') {
      //   if(targetController == '1') pixels1.setPixelColor(lightIndex, pixels1.Color(buttonReds[lightIndex], buttonGreens[lightIndex], buttonBlues[lightIndex]));
      //   if(targetController == '2') pixels2.setPixelColor(lightIndex, pixels2.Color(buttonReds[lightIndex], buttonGreens[lightIndex], buttonBlues[lightIndex]));
      //   if(targetController == '3') pixels3.setPixelColor(lightIndex, pixels3.Color(buttonReds[lightIndex], buttonGreens[lightIndex], buttonBlues[lightIndex]));
      //   if(targetController == '4') pixels4.setPixelColor(lightIndex, pixels4.Color(buttonReds[lightIndex], buttonGreens[lightIndex], buttonBlues[lightIndex]));
      // }
      // else {
      //   if(targetController == '1') pixels1.setPixelColor(lightIndex, pixels1.Color(20, 20, 20));
      //   if(targetController == '2') pixels2.setPixelColor(lightIndex, pixels2.Color(20, 20, 20));
      //   if(targetController == '3') pixels3.setPixelColor(lightIndex, pixels3.Color(20, 20, 20));
      //   if(targetController == '4') pixels4.setPixelColor(lightIndex, pixels4.Color(20, 20, 20));
      // }
    // }
    
    pixels1.show();
    pixels2.show();
    pixels3.show();
    pixels4.show();
}
}

void loop() {
  bool buttonsChanged =checkButtons();
  delay(50);
  updateLights(buttonsChanged);
  delay(50);

}

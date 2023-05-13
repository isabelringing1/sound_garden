#include "cstdlib"
#include "Adafruit_NeoPixel.h"

using namespace std;

// which pins are connected to lights?
const int button1LightPin = 14;
const int button2LightPin = 15;
const int button3LightPin = 16;
const int button4LightPin = 17;
const int button5LightPin = 18;
const int button6LightPin = 19;
const int button7LightPin = 20;
const int button8LightPin = 21;


const int lightsPin = 10;

// which pins are connected to buttons?
const int button1Pin = 9;
const int button2Pin = 8;
const int button3Pin = 7;
const int button4Pin = 6;
const int button5Pin = 5;
const int button6Pin = 4;
const int button7Pin = 3;
const int button8Pin = 2;

const int numButtons = 8;
const int buttonLEDPins[] = {button1LightPin, button2LightPin, button3LightPin, button4LightPin,
                             button5LightPin, button6LightPin, button7LightPin, button8LightPin};

const int buttonPins[] = {button1Pin, button2Pin, button3Pin, button4Pin,
                             button5Pin, button6Pin, button7Pin, button8Pin};

String lastState = "0,0,0,0,0,0,0,0";

Adafruit_NeoPixel pixels(numButtons, lightsPin, NEO_GRB + NEO_KHZ800);

const int buttonReds[] = {255, 255, 255, 119, 0, 0, 188, 255};
const int buttonGreens[] = {0, 154, 239, 255, 255, 60, 0, 0};
const int buttonBlues[] = {0, 0, 0, 0, 252, 255, 255, 0};

void setup() {

 // configure the digital input:
  pinMode(button1Pin, INPUT);
  pinMode(button2Pin, INPUT);
  pinMode(button3Pin, INPUT);
  pinMode(button4Pin, INPUT);
  pinMode(button5Pin, INPUT);
  pinMode(button6Pin, INPUT);
  pinMode(button7Pin, INPUT);
  pinMode(button8Pin, INPUT);

   // configure the digital output:
  pinMode(button1LightPin, OUTPUT);
  pinMode(button2LightPin, OUTPUT);
  pinMode(button3LightPin, OUTPUT);
  pinMode(button4LightPin, OUTPUT);
  pinMode(button5LightPin, OUTPUT);
  pinMode(button6LightPin, OUTPUT);
  pinMode(button7LightPin, OUTPUT);
  pinMode(button8LightPin, OUTPUT);

  Serial.setTimeout(50);
  Serial.begin(9600); 
  pixels.begin();

  testNeopixels();
}

 // run through LED pins to check they're all connected
void checkLEDPins() {

    // first, turn all lights off
    for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        digitalWrite(buttonLEDPins[pinNumber], LOW);
    }

  for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
    digitalWrite(buttonLEDPins[pinNumber], HIGH);
    delay(500);
  }
}

// turn on all the connected neopixels
void testNeopixels() {
  for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels.setPixelColor(i, pixels.Color(255, 0, 0));
    pixels.show();  
  }

  delay(1000);
  for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels.setPixelColor(i, pixels.Color(0, 255, 0));
    pixels.show();  
  }
    delay(1000);

    for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels.setPixelColor(i, pixels.Color(0, 0, 255));
    pixels.show();  
  }

      delay(1000);

      for(int i=0; i<numButtons; i++) { // For each pixel...
    pixels.setPixelColor(i, pixels.Color(255, 255, 255));
    pixels.show();  

    delay(1000);

    pixels.clear()
    pixels.show();  


  }


}


 // check buttons and write values to serial
void checkButtons() {
    String curr = "";
    for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins[pinNumber]);
        curr += String(sensorValue);
        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          curr += ",";
        }
    }
    if (!curr.equals(lastState)){
      Serial.println(curr);
    }
    lastState = curr;
}

 // check serial data for light info and update lights accordingly
void updateLights() {
    if (Serial.available() >= numButtons + 1) {
      String lightInputString = Serial.readStringUntil('/n');
      pixels.clear();
      for(int lightIndex = 0; lightIndex < lightInputString.length(); lightIndex++) {
        int sensorValue = digitalRead(buttonPins[lightIndex]);
        if(lightInputString.charAt(lightIndex) == '1') {
            pixels.setPixelColor(lightIndex, pixels.Color(buttonReds[lightIndex], buttonGreens[lightIndex], buttonBlues[lightIndex]));
        }
      }
      pixels.show();
  }
}

void loop() {
  updateLights();
  delay(50);
  checkButtons();
  delay(50);

}

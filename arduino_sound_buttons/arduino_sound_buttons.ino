#include "cstdlib"

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

   
  Serial.begin(9600); 
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


 // check buttons and write values to serial
void checkButtons() {

    for(int pinNumber = 0; pinNumber < numButtons; pinNumber++) {
        int sensorValue = digitalRead(buttonPins[pinNumber]);

        // if we are before the last button
        if(pinNumber < numButtons - 1) {
          Serial.print(sensorValue);
          Serial.print(", ");
        }

        // if we are at the last button
        else {
          Serial.println(sensorValue);
        }

      // turn on light if button is pushed
      digitalWrite(buttonLEDPins[pinNumber], sensorValue);

    }
}

void loop() {

    checkButtons();
    delay(20);

  }

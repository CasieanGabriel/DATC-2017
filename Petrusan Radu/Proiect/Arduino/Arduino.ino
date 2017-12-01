const int TRIG_PIN = 4;
const int ECHO_PIN = 5;
const int GREENLED_PIN = 8;
const int REDLED_PIN = 7;
// Anything over 400 cm (23200 us pulse) is "out of range"
const unsigned int MAX_DIST = 23200;

void setup() {

  // The Trigger pin will tell the sensor to range find
  pinMode(TRIG_PIN, OUTPUT);
  digitalWrite(TRIG_PIN, LOW);

  pinMode(GREENLED_PIN, OUTPUT);
  pinMode(REDLED_PIN, OUTPUT);
  // We'll use the serial monitor to view the sensor output
  digitalWrite(GREENLED_PIN, HIGH);
  Serial.begin(9600);
}

void loop() {

  unsigned long t1;
  unsigned long t2;
  unsigned long debounce_time1;
  unsigned long debounce_time2;
  unsigned long pulse_width;
  float cm;

  // Hold the trigger pin high for at least 10 us
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);

  // Wait for pulse on echo pin
  while ( digitalRead(ECHO_PIN) == 0 );

  // Measure how long the echo pin was held high (pulse width)
  // Note: the micros() counter will overflow after ~70 min
  t1 = micros();
  while ( digitalRead(ECHO_PIN) == 1);
  t2 = micros();
  pulse_width = t2 - t1;

  // Calculate distance in centimeters and inches. The constants
  // are found in the datasheet, and calculated from the assumed speed 
  //of sound in air at sea level (~340 m/s).
  cm = pulse_width / 58.0;

  // Print out results
  if ( pulse_width > MAX_DIST ) 
  {
    Serial.println("Out of range");
  } 
  else 
  {
    Serial.print(cm);
    Serial.println(" cm \t");
    if(cm < 10)
    {
      digitalWrite(GREENLED_PIN, LOW);
      digitalWrite(REDLED_PIN, HIGH);
    }
    else
    { 
      digitalWrite(REDLED_PIN, LOW);
      digitalWrite(GREENLED_PIN, HIGH);
    }
  }
  
  // Wait at least 60ms before next measurement
  delay(60);
}

//Message
byte input_byte;
//Setup
void setup(){
  //Serial baud rate 9600.
  Serial.begin(9600);
}
//Main loop.
void loop(){
  //Buffer size is 1 byte.
  if(Serial.available() == 1){
    //Read from buffer.
    input_byte = Serial.read();
  }
  if(input_byte == 0){
    Serial.print("THIS IS ARDUINO!");
    return;
  }
  //Scores
  byte left_score = input_byte >> 4;
  byte right_score = input_byte & B00001111;
  //Clear message.
  input_byte = 0;
  Serial.print("READY TO COMMUNICATE");
}

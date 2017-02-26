
#include <Servo.h>
Servo myservo;  // create servo object to control a servo
const int PWM=9;

#include <Stepper.h>
const int stepsPerRevolution = 400;  // change this to fit the number of steps per revolution
Stepper myStepper(stepsPerRevolution, 2, 3, 4, 5);

const int Red=11;
const int Green=12;
const int Blue=13;
const int Led = 10;

char serialdata[15]={48,48,48,48,48};// initialize with all 0
int motor,servo,pos = 20; 

void setup() {
 Serial.begin(9600);
 Serial.setTimeout(50);
 
 myservo.attach(PWM);  // attaches the servo on pin 6 to the servo object
 
 myStepper.setSpeed(50);
  
 pinMode(Red,OUTPUT);
 pinMode(Green,OUTPUT);
 pinMode(Blue,OUTPUT);
 pinMode(Led,OUTPUT);


digitalWrite(Red,HIGH);
digitalWrite(Green,HIGH);
digitalWrite(Blue,HIGH);
}

void loop() {
int val1,val2,val3,val4,val5,lf = 10,i=0;

 if(Serial){ //if doesnt found usb open, do not read!
 Serial.readBytesUntil(lf, serialdata, 5);
 
// for (i=0; i<15;i++){
//  Serial.print(serialdata[i]); //send eco
// } 
// Serial.println();
}
//Download Buffer
val1=(int)serialdata[0];
val2=(int)serialdata[1];
val3=(int)serialdata[2];
val4=(int)serialdata[3];
val5=(int)serialdata[4];
//LED
if (val1 == 'A' && val2 == 'L'){ //AL1/AL0
  if(val3=='1'){
    digitalWrite(Led,HIGH);
    Serial.println("AL1"); 
  }
  if(val3=='0'){
  digitalWrite(Led,LOW);
  Serial.println("AL0");
 } 
}

//SERVO
if (val1 == 65 && val2 == 83){//AS1/AS0
  if(val3==49){
   Serial.println("AS1"); //send eco
    servo = 1;
      }
if(val3==48){
  Serial.println("AS0"); 
    servo = 0;          
  }
}
if(servo == 1){
 for (pos = pos; pos <= 50; pos += 2) { 
    myservo.write(pos);              
    delay(10);                       
             } 
  for (pos = 170; pos >= 20; pos -= 2) { 
    myservo.write(pos);              
   delay(10);                       
  }
 }

//STEP
if (val1 == 65 && val2 == 77){//AM1/AM0
  if (val3 == 48 ){
    Serial.println("AM0");
    motor = 0;
  }
    if (val3 == 49){
  Serial.println("AM1");
  motor = 1;
          }
}
 if (motor == 1){
  myStepper.step(-400);
  delay(200); 
 }

//LED-RGB

if(val1==65 && val2==82){          //AR-0,1,2,3,4,5,6,7.
if(val3==55){                         
digitalWrite(Red,LOW);
digitalWrite(Green,LOW);
digitalWrite(Blue,LOW);
Serial.println("AR7"); //send eco
}
  if(val3==54){                         
digitalWrite(Red,HIGH);
digitalWrite(Green,LOW);
digitalWrite(Blue,LOW);
Serial.println("AR6"); //send eco
}
if(val3==53){                         
digitalWrite(Red,LOW);
digitalWrite(Green,HIGH);
digitalWrite(Blue,LOW);
Serial.println("AR5"); //send eco
}
if(val3==52){                         
digitalWrite(Red,HIGH);
digitalWrite(Green,HIGH);
digitalWrite(Blue,LOW);
Serial.println("AR4"); //send eco
}
if(val3==51){                         
digitalWrite(Red,LOW);
digitalWrite(Green,LOW);
digitalWrite(Blue,HIGH);
Serial.println("AR3"); //send eco
}
if(val3==50){                         
digitalWrite(Red,HIGH);
digitalWrite(Green,LOW);
digitalWrite(Blue,HIGH);
Serial.println("AR2"); //send eco
}
if(val3==49){                         
digitalWrite(Red,LOW);
digitalWrite(Green,HIGH);
digitalWrite(Blue,HIGH);
Serial.println("AR1"); //send eco
}
if(val3==48){                         
digitalWrite(Red,HIGH);
digitalWrite(Green,HIGH);
digitalWrite(Blue,HIGH);
Serial.println("AR0"); //send eco
      }
    }

/*   OFF = 0
 *   B   = 1 
 *   G   = 2
 *   GB  = 3
 *   R   = 4
 *   RB  = 5
 *   RG  = 6 
 *   RGB = 7 
 */
   //RESET BUFFER
serialdata[0]=0;
serialdata[1]=0;
serialdata[2]=0;
serialdata[3]=0;
serialdata[4]=0;


  }


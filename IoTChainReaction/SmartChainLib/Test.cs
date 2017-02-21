using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartChainLib
{
    class Test
    {
        static WhiteCube m_WhiteCube;
        static Arduino m_Arduino;

        static Test()
        {
            m_Arduino = new Arduino();
            m_WhiteCube = new WhiteCube();

            m_Arduino.SetLED(eLEDState.Off);
            m_Arduino.SetStepMotor(eStepMotorState.Off);
            m_Arduino.SetServoMotor(eServoMotorState.Deg0);
            m_Arduino.SetRGBLED(eRGBLEDState.Off);

            //Actuators:
            m_Arduino.LEDStateChange += Ard_LEDStateChange;
            m_Arduino.RGBLEDStateChange += Ard_RGBLEDStateChange;
            m_Arduino.StepMotorStateChange += Ard_StepMotorStateChange;
            m_Arduino.ServoMotorStateChange += Ard_ServoMotorStateChange;

            //Sensors:
            m_WhiteCube.ButtonSensorStateChange += M_WhiteCube_ButtonSensorStateChange;
            m_WhiteCube.ReedSensorStateChange += M_WhiteCube_ReedSensorStateChange;
            m_WhiteCube.LightSensorStateChange += M_WhiteCube_LightSensorStateChange;
            m_WhiteCube.DTHSensorStateChange += M_WhiteCube_DTHSensorStateChange;

            m_WhiteCube.ButtonSensorStateChange += Button_Clicked;
            
        }

        public static void Main()
        {

            //m_Arduino = new Arduino();
            //ard.AutoDetectArduinoPort();
            /*
            foreach (var i in (new int[] { 1, 2, 3, 4, 5, 6 }))
            {
                m_Arduino.WriteLine("AL0");
                Thread.Sleep(1000);
                m_Arduino.WriteLine("AL1");
                Thread.Sleep(1000);
            }
            */
            //Console.ReadLine();
            

            


            while(true)
            {
                //
            }
        }

        private static void M_WhiteCube_DTHSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            Console.WriteLine("Event: Temperature {0}, Humidity {1}", i_Tempeprature, i_Humidity);
            //update GUI
        }

        private static void M_WhiteCube_LightSensorStateChange(int i_Value)
        {
            Console.WriteLine("Event: Light {0}", i_Value);
            //update GUI
        }

        private static void M_WhiteCube_ReedSensorStateChange()
        {
            Console.WriteLine("Event: Reed");
            //update GUI
        }

        private static void M_WhiteCube_ButtonSensorStateChange()
        {
            Console.WriteLine("Event: Button");
            //update GUI
        }
        
        private static void Ard_ServoMotorStateChange(eServoMotorState i_State)
        {
            Console.WriteLine("Event: Servo {0}", i_State);
            //update GUI
        }

        private static void Ard_StepMotorStateChange(eStepMotorState i_State)
        {
            Console.WriteLine("Event: StepMotor {0}", i_State);
            //update GUI
        }

        private static void Ard_RGBLEDStateChange(eRGBLEDState i_State)
        {
            Console.WriteLine("Event: RGBLED {0}", i_State);
            //update GUI
        }

        private static void Ard_LEDStateChange(eLEDState i_State)
        {
            Console.WriteLine("Event: LED {0}", i_State);
            //update GUI
        }

        #region sensors
        public static void Button_Clicked()
        {
            Console.WriteLine("SmartChain: Button");
            m_WhiteCube.ButtonSensorStateChange -= Button_Clicked;
            m_Arduino.SetLED(eLEDState.On);
            m_WhiteCube.LightSensorStateChange += Light_Sensed;
        }

        public static void Light_Sensed(int i_Value)
        {
            Console.WriteLine("SmartChain: Light {0}", i_Value);
            m_WhiteCube.LightSensorStateChange -= Light_Sensed;
            m_Arduino.SetStepMotor(eStepMotorState.On);
            m_WhiteCube.ReedSensorStateChange += ReedMagnet_Sensed;
        }

        public static void ReedMagnet_Sensed()
        {
            Console.WriteLine("SmartChain: ReedMagnet");

            m_WhiteCube.ReedSensorStateChange -= ReedMagnet_Sensed;
            m_Arduino.SetStepMotor(eStepMotorState.Off);
            m_Arduino.SetServoMotor(eServoMotorState.Deg180);
            m_WhiteCube.DTHSensorStateChange += DTH_Sensed;
        }

        public static void DTH_Sensed(float i_Temperature, float i_Humidity)
        {
            Console.WriteLine("SmartChain: DTH Temp={0}, Humid={1}", i_Temperature, i_Humidity);

            m_WhiteCube.DTHSensorStateChange -= DTH_Sensed;
            m_Arduino.SetRGBLED(eRGBLEDState.GB);//value need to depend on both inputs (R - hot and humid, B - cold and dry....)
        }
        #endregion
        
    }
}

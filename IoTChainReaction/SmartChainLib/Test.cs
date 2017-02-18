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
            m_WhiteCube.ButtonSensorStateChange += Button_Clicked;
        }

        public static void Main()
        {
            m_Arduino = new Arduino();
            //ard.AutoDetectArduinoPort();
            foreach (var i in (new int[] { 1, 2, 3, 4, 5, 6 }))
            {
                m_Arduino.WriteLine("AL0");
                Thread.Sleep(1000);
                m_Arduino.WriteLine("AL1");
                Thread.Sleep(1000);
            }
            //Console.ReadLine();

            //Actuators
            m_Arduino.LEDStateChange += Ard_LEDStateChange;
            m_Arduino.RGBLEDStateChange += Ard_RGBLEDStateChange;
            m_Arduino.StepMotorStateChange += Ard_StepMotorStateChange;
            m_Arduino.ServoMotorStateChange += Ard_ServoMotorStateChange;

            //Sensors:
            m_WhiteCube.ButtonSensorStateChange += M_WhiteCube_ButtonSensorStateChange;
            m_WhiteCube.ReedSensorStateChange += M_WhiteCube_ReedSensorStateChange;
            m_WhiteCube.LightSensorStateChange += M_WhiteCube_LightSensorStateChange;
            m_WhiteCube.DHTSensorStateChange += M_WhiteCube_DHTSensorStateChange;
        }

        private static void M_WhiteCube_DHTSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void M_WhiteCube_LightSensorStateChange(int i_Value)
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void M_WhiteCube_ReedSensorStateChange()
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void M_WhiteCube_ButtonSensorStateChange()
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void Ard_ServoMotorStateChange(eServoMotorState i_State)
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void Ard_StepMotorStateChange(eStepMotorState i_State)
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void Ard_RGBLEDStateChange(eRGBLEDState i_State)
        {
            throw new NotImplementedException();
            //update GUI
        }

        private static void Ard_LEDStateChange(eLEDState i_State)
        {
            throw new NotImplementedException();
            //update GUI
        }

        #region sensors
        public static void Button_Clicked()
        {
            m_WhiteCube.ButtonSensorStateChange -= Button_Clicked;
            m_Arduino.SetLED(eLEDState.On);
            m_WhiteCube.LightSensorStateChange += Light_Sensed;
        }

        public static void Light_Sensed(int i_Value)
        {
            m_WhiteCube.LightSensorStateChange -= Light_Sensed;
            m_Arduino.SetServoMotor(eServoMotorState.Deg180);
            m_WhiteCube.ReedSensorStateChange += ReedMagnet_Sensed;
        }

        public static void ReedMagnet_Sensed()
        {
            m_WhiteCube.ReedSensorStateChange -= ReedMagnet_Sensed;
            m_WhiteCube.DHTSensorStateChange += DHT_Sensed;
            m_Arduino.SetStepMotor(eStepMotorState.On);
        }

        public static void DHT_Sensed(float i_Temperature, float i_Humidity)
        {
            m_WhiteCube.DHTSensorStateChange -= DHT_Sensed;
            m_Arduino.SetRGBLED(eRGBLEDState.RGB);//value need to depend on both inputs (R - hot and humid, B - cold and dry....)
        }
        #endregion
        
    }
}

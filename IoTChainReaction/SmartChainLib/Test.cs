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
        //WhiteCube m_WhiteCube;
        public static void Main()
        {
            Arduino ard = new Arduino();
            //ard.AutoDetectArduinoPort();
            foreach (var i in (new int[] { 1, 2, 3, 4, 5, 6 }))
            {
                ard.WriteLine("AL0");
                Thread.Sleep(1000);
                ard.WriteLine("AL1");
                Thread.Sleep(1000);
            }
            //Console.ReadLine();

            ard.LEDStateChange += Ard_LEDStateChange;
            ard.RGBLEDStateChange += Ard_RGBLEDStateChange;
            ard.StepMotorStateChange += Ard_StepMotorStateChange;
            ard.ServoMotorStateChange += Ard_ServoMotorStateChange;

        }

        private static void Ard_ServoMotorStateChange(eServoMotorState i_State)
        {
            throw new NotImplementedException();
        }

        private static void Ard_StepMotorStateChange(eStepMotorState i_State)
        {
            throw new NotImplementedException();
        }

        private static void Ard_RGBLEDStateChange(eRGBLEDState i_State)
        {
            throw new NotImplementedException();
        }

        private static void Ard_LEDStateChange(eLEDState i_State)
        {
            throw new NotImplementedException();
        }

        Test()
        {
            m_WhiteCube = new WhiteCube();
            m_WhiteCube.ButtonClick += OnButtonClicked;
        }
        #region actuator
        public void SetLightScreen(int state)
        {

        }

        public void SetServoMotor(int state)
        {

        }

        public void SetRGB(int state)
        {

        }

        public void SetStepMotor(int state)
        {

        }
        #endregion

        #region sensors
        public void OnButtonClicked(object sender, EventArgs e)
        {
            m_WhiteCube.ButtonClick -= m_WhiteCube.OnButtonClicked;
            SetLightScreen(50);
            m_WhiteCube.LightSensorEvent += OnLightSensed;

        }

        public void OnLightSensed(object sender, EventArgs e)
        {
            m_WhiteCube.LightSensorEvent -= OnLightSensed;
            SetServoMotor(45);
            m_WhiteCube.ReedSensorEvent += OnReedMagnetSensed;
        }

        public void OnReedMagnetSensed(object sender, EventArgs e)
        {
            m_WhiteCube.ReedSensorEvent -= OnReedMagnetSensed;
            m_WhiteCube.DHTSensorEvent += OnDHTSensed;
            SetStepMotor(1);
        }

        public void OnDHTSensed(object sender, EventArgs e)
        {
            m_WhiteCube.DHTSensorEvent -= OnDHTSensed;
            SetRGB(0);
        }

        public void OnButtonClickedAgain(object sender, EventArgs e)
        {
            StopAndResetEverything();
        }
        #endregion
        
    }
}

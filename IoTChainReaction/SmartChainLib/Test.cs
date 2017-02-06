using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartChainLib
{
    class Test
    {
        WhiteCube m_WhiteCube;
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

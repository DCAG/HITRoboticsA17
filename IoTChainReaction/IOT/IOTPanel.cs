using SmartChainLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOT
{
    delegate void UpdateTextBox(Control ctrl, string str);
    public partial class IOTPanel : Form
    {
        Timer m_ActuatorsReadyTimer;

        private Timer m_SmartChainTimer;
        private TimeSpan m_SmartChainRunTime;
        private Arduino m_Arduino;
        private WhiteCube m_WhiteCube;
        private bool m_SmartChainActive;

        public IOTPanel()
        {
            InitializeComponent();
            m_SmartChainActive = false;
            m_ActuatorsReadyTimer = new Timer();
            m_ActuatorsReadyTimer.Interval = 200;
            m_ActuatorsReadyTimer.Tick += M_ActuatorsReadyTimer_Tick;

            m_SmartChainTimer = new Timer();
            m_SmartChainTimer.Interval = 1000;
            m_SmartChainTimer.Tick += m_SmartChainTimer_Tick;
            m_SmartChainRunTime = new TimeSpan(0, 0, 0);

            m_Arduino = new Arduino();
            m_Arduino.RGBLEDStateChange += M_Arduino_RGBLEDStateChange;
            m_Arduino.LEDStateChange += M_Arduino_LEDStateChange;
            m_Arduino.ServoMotorStateChange += M_Arduino_ServoMotorStateChange;
            m_Arduino.StepMotorStateChange += M_Arduino_StepMotorStateChange;

            m_WhiteCube = new WhiteCube();
            m_WhiteCube.LightSensorStateChange += M_WhiteCube_LightSensorStateChange;
            m_WhiteCube.ButtonSensorStateChange += M_WhiteCube_ButtonSensorStateChange;
            m_WhiteCube.ReedSensorStateChange += M_WhiteCube_ReedSensorStateChange;
            m_WhiteCube.DTHSensorStateChange += M_WhiteCube_DTHSensorStateChange;

            resetActuators();
        }

        private void m_SmartChainTimer_Tick(object sender, EventArgs e)
        {
            m_SmartChainRunTime = m_SmartChainRunTime.Add(new TimeSpan(0, 0, 1));
            RunTimeLabel.Text = m_SmartChainRunTime.ToString();
        }

        private void startSmartChainButton_Click(object sender, EventArgs e)
        {
            if (startSmartChainButton.Text != "Stop") // start chain
            {
                ActuatorsGroupBox.Enabled = false;
                resetActuators();
                m_ActuatorsReadyTimer.Start();
            }
            else
            {
                endSmartChain();
            }
        }

        private void startChainWhenActuatorsReady()
        {
            m_SmartChainRunTime = new TimeSpan(0, 0, 0);
            m_SmartChainActive = true;
            m_WhiteCube.ButtonSensorStateChange += SmartChainButton_Clicked; // timer starts here - when button is clicked
            startSmartChainButton.Text = "Stop";

        }

        private void M_ActuatorsReadyTimer_Tick(object sender, EventArgs e)
        {
            if (RgbTextBox.Text != "Off" ||
            StepMotorTextBox.Text != "Off" ||
            ServoMotorTextBox.Text != "Off" ||
            LEDTextBox.Text != "Off")
            {
                return; // wait for next tick, actuators are not ready.
            }
            else
            {
                m_ActuatorsReadyTimer.Stop();
                startChainWhenActuatorsReady();
            }
        }

        private void IOTPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            //m_Arduino.Disconnect();
            //m_WhiteCube.Disconnect();
        }

        private void resetActuators()
        {
            if(LEDTextBox.Text != "Off")
                m_Arduino.SetLED(eLEDState.Off);
            if(ServoMotorTextBox.Text != "Off")
                m_Arduino.SetServoMotor(eServoMotorState.Off);
            if(StepMotorTextBox.Text != "Off")
                m_Arduino.SetStepMotor(eStepMotorState.Off);
            if(RgbTextBox.Text != "Off")
                m_Arduino.SetRGBLED(eRGBLEDState.Off);
        }

        /// <summary>
        /// Helper function that updates Control's text attribute immedietly.
        /// </summary>
        /// <param name="ctrl">the control to update</param>
        /// <param name="str">set text to this input</param>
        void UpdateControlsText(Control ctrl, string str)
        {
            if(ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateTextBox(UpdateControlsText), ctrl, str);
            }
            else
            {
                ctrl.Text = str;
            }
        }

        // Sensors status updaters

        private void M_WhiteCube_DTHSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            UpdateControlsText(DTHSensorTextBox, "Temp:" + i_Tempeprature + "\r\n" + "Hum:" + i_Humidity);
        }

        private void M_WhiteCube_ReedSensorStateChange()
        {
            UpdateControlsText(ReedSensorTextBox , DateTime.Now.ToLongTimeString());
        }

        private void M_WhiteCube_ButtonSensorStateChange()
        {
            UpdateControlsText(ButtonSensorTextBox, DateTime.Now.ToLongTimeString());
        }

        private void M_WhiteCube_LightSensorStateChange(int i_Value)
        {
            UpdateControlsText(LightSensorTextBox , i_Value.ToString());
        }

        //Actuators status updaters

        private void M_Arduino_StepMotorStateChange(eStepMotorState i_State)
        {
            if (StepMotorTextBox.InvokeRequired)
            {
                StepMotorTextBox.Invoke(new StepMotorStateChangeDelegate(M_Arduino_StepMotorStateChange), i_State);
            }
            else
            {
                UpdateControlsText(StepMotorTextBox, i_State.ToString());
                if (m_SmartChainActive)
                    StepMotorTextBox.BackColor = Color.Yellow;
            }
        }

        private void M_Arduino_ServoMotorStateChange(eServoMotorState i_State)
        {
            if (ServoMotorTextBox.InvokeRequired)
            {
                ServoMotorTextBox.Invoke(new ServoMotorStateChangeDelegate(M_Arduino_ServoMotorStateChange), i_State);
            }
            else
            {
                UpdateControlsText(ServoMotorTextBox, i_State.ToString());
                if (m_SmartChainActive)
                    ServoMotorTextBox.BackColor = Color.Yellow;
            }
        }

        private void M_Arduino_LEDStateChange(eLEDState i_State)
        {
            if (LedButton.InvokeRequired)
            {
                LedButton.Invoke(new LEDStateChangeDelegate(M_Arduino_LEDStateChange), i_State);
            }
            else
            {
                UpdateControlsText(LEDTextBox, i_State.ToString());
                if (i_State == eLEDState.On)
                {
                    LedButton.BackColor = Color.Red;
                }
                else
                {
                    LedButton.BackColor = Color.DarkGray;
                }

                if (m_SmartChainActive)
                    LEDTextBox.BackColor = Color.Yellow;
            }
        }

        private void M_Arduino_RGBLEDStateChange(eRGBLEDState i_State)
        {
            if (RgbTextBox.InvokeRequired)
            {
                RgbTextBox.Invoke(new RGBLEDStateChangeDelegate(M_Arduino_RGBLEDStateChange), i_State);
            }
            else
            {
                UpdateControlsText(RgbTextBox, i_State.ToString());
                if (m_SmartChainActive)
                {
                    RgbTextBox.BackColor = Color.Yellow;
                    endSmartChain();
                }
            }
        }

        private void endSmartChain()
        {
            m_SmartChainActive = false;
            m_SmartChainTimer.Stop();
            startSmartChainButton.Text = "Start Smart Chain";

            RgbTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            StepMotorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            ServoMotorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            LEDTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);

            DTHSensorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            LightSensorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            ButtonSensorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);
            ReedSensorTextBox.BackColor = Color.FromKnownColor(KnownColor.Control);

            ActuatorsGroupBox.Enabled = true;
        }

        //Actuators buttons

        private void LEDButton_Click(object sender, EventArgs e)
        {
            if (LEDTextBox.Text == "On")
            {
                m_Arduino.SetLED(eLEDState.Off);
            }
            else if ((LEDTextBox.Text == "Off"))
            {
                m_Arduino.SetLED(eLEDState.On);
            }
        }

        private void StepMotorButton_Click(object sender, EventArgs e)
        {
            if (StepMotorTextBox.Text == "On")
                m_Arduino.SetStepMotor(eStepMotorState.Off);
            else if (StepMotorTextBox.Text == "Off")
                m_Arduino.SetStepMotor(eStepMotorState.On);
        }

        private void ServoMotorButton_Click(object sender, EventArgs e)
        {
            if (ServoMotorTextBox.Text == "On")
                m_Arduino.SetServoMotor(eServoMotorState.Off);
            else if (ServoMotorTextBox.Text == "Off")
                m_Arduino.SetServoMotor(eServoMotorState.On);
        }

        #region RGBLed Buttons
        private void RedButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Red);
        }

        private void AzureButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Azure);
        }

        private void YellowButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Yellow);
        }

        private void TurnOffRGBLedButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Off);
        }

        private void PinkButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Pink);
        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Blue);
        }

        private void WhiteButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.White);
        }

        private void GreenButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Green);
        }
        #endregion
        
        //Smart Chain:
        #region Smart Chain
        private void SmartChainButton_Clicked()
        {
            if (ButtonSensorTextBox.InvokeRequired)
            {
                ButtonSensorTextBox.Invoke(new ButtonSensorStateChangeDelegate(SmartChainButton_Clicked));
            }
            else
            {
                ButtonSensorTextBox.BackColor = Color.Yellow;
                m_SmartChainTimer.Start();
                m_WhiteCube.ButtonSensorStateChange -= SmartChainButton_Clicked;
                m_Arduino.SetLED(eLEDState.On);
                m_WhiteCube.LightSensorStateChange += SmartChainLight_Sensed;
            }
        }

        public void SmartChainLight_Sensed(int i_Value)
        {
            if (LightSensorTextBox.InvokeRequired)
            {
                LightSensorTextBox.Invoke(new LightSensorStateChangeDelegate(SmartChainLight_Sensed), i_Value);
            }
            else
            {
                LightSensorTextBox.BackColor = Color.Yellow;
                m_WhiteCube.LightSensorStateChange -= SmartChainLight_Sensed;
                m_Arduino.SetStepMotor(eStepMotorState.On);
                m_WhiteCube.ReedSensorStateChange += SmartChainReedMagnet_Sensed;
            }
        }

        public void SmartChainReedMagnet_Sensed()
        {
            ReedSensorTextBox.BackColor = Color.Yellow;
            m_WhiteCube.ReedSensorStateChange -= SmartChainReedMagnet_Sensed;
            m_Arduino.SetStepMotor(eStepMotorState.Off);
            m_Arduino.SetServoMotor(eServoMotorState.On);
            m_WhiteCube.DTHSensorStateChange += SmartChainDTH_Sensed;
        }

        public void SmartChainDTH_Sensed(float i_Temperature, float i_Humidity)
        {
            DTHSensorTextBox.BackColor = Color.Yellow;
            m_WhiteCube.DTHSensorStateChange -= SmartChainDTH_Sensed;
            if( i_Temperature + i_Humidity > 60 )
                m_Arduino.SetRGBLED(eRGBLEDState.Yellow);
            else if( i_Temperature + i_Humidity > 55 )
                m_Arduino.SetRGBLED(eRGBLEDState.Red);
            else if( i_Temperature + i_Humidity > 50 )
                m_Arduino.SetRGBLED(eRGBLEDState.Pink);
            else if( i_Temperature + i_Humidity > 45 )
                m_Arduino.SetRGBLED(eRGBLEDState.Green);
            else if( i_Temperature + i_Humidity > 40 )
                m_Arduino.SetRGBLED(eRGBLEDState.Azure);
            else if( i_Temperature + i_Humidity > 35 )
                m_Arduino.SetRGBLED(eRGBLEDState.Blue);
            else
                m_Arduino.SetRGBLED(eRGBLEDState.White);
        }
        #endregion

        private void resetButton_Click(object sender, EventArgs e)
        {
            //reset all text boxes...
            //resetActuators();
            
        }
    }
}

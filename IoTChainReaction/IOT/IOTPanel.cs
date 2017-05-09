using SmartChainLib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace IOT
{
    delegate void UpdateTextBox(Control ctrl, string str);

    public partial class IOTPanel : Form
    {
        readonly Color r_SmartChainBackColor = Color.LightGreen;

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

            /*
             * timer that prepares the actuators for the SmartChain reaction
             */
            m_ActuatorsReadyTimer = new Timer();
            m_ActuatorsReadyTimer.Interval = 200;
            m_ActuatorsReadyTimer.Tick += M_ActuatorsReadyTimer_Tick;

            /*
             * timer that counts time and updates the label under 'run time' when during SmartChain   
             */
            m_SmartChainTimer = new Timer();
            m_SmartChainTimer.Interval = 1000;
            m_SmartChainTimer.Tick += m_SmartChainTimer_Tick;
            m_SmartChainRunTime = new TimeSpan(0, 0, 0);

            /*
             * initialize Arduino object and subscribe to all of its notifications
             */
            m_Arduino = new Arduino();
            m_Arduino.ArduinoConnectionStatusChange += M_Arduino_ArduinoConnectionStatusChange;
            m_Arduino.RGBLEDStateChange += M_Arduino_RGBLEDStateChange;
            m_Arduino.LEDStateChange += M_Arduino_LEDStateChange;
            m_Arduino.ServoMotorStateChange += M_Arduino_ServoMotorStateChange;
            m_Arduino.StepMotorStateChange += M_Arduino_StepMotorStateChange;

            /*
             * initialize WhiteCube object and subscribe to all of its notifications
             */
            m_WhiteCube = new WhiteCube();
            m_WhiteCube.DeviceIdentificationNotification += M_WhiteCube_SensorIdentificationNotification;
            m_WhiteCube.WhiteCubeConnectionStatusChange += M_WhiteCube_WhiteCubeConnectionStatusChange;
            m_WhiteCube.LightSensorStateChange += M_WhiteCube_LightSensorStateChange;
            m_WhiteCube.ButtonSensorStateChange += M_WhiteCube_ButtonSensorStateChange;
            m_WhiteCube.ReedSensorStateChange += M_WhiteCube_ReedSensorStateChange;
            m_WhiteCube.DTHSensorStateChange += M_WhiteCube_DTHSensorStateChange;
        }

        /// <summary>
        /// invoked whenever identify|exitence|ping message is sent
        /// e.g.:
        /// { "device_name":"3PI_1152728", "type":"led", "ipaddress":"192.168.8.116", "bgn":3, "uptime":77, "sdk":"1.4.0", "version":"0.2.1" }
        /// </summary>
        /// <param name="i_Sensor"></param>
        private void M_WhiteCube_SensorIdentificationNotification(eWhiteCubeDevice i_Sensor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DeviceIdentificationNotificationDelegate(M_WhiteCube_SensorIdentificationNotification), i_Sensor);
            }
            else
            {
                switch (i_Sensor)
                {
                    case eWhiteCubeDevice.button:
                        buttonSensorIdTimeLabel.Text = DateTime.Now.ToLongTimeString();
                        break;
                    case eWhiteCubeDevice.light:
                        lightSensorIdTimeLabel.Text = DateTime.Now.ToLongTimeString();
                        break;
                    case eWhiteCubeDevice.dth:
                        dthSensorIdTimeLabel.Text = DateTime.Now.ToLongTimeString();
                        break;
                    case eWhiteCubeDevice.reed:
                        reedSensorIdTimeLabel.Text = DateTime.Now.ToLongTimeString();
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// invoked when form is loading
        /// connect to whitecube and arduino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IOTPanel_Load(object sender, EventArgs e)
        {
            m_Arduino.StartAutoReconnect();
            if (m_Arduino.IsConnectedToComputer)
            {
                m_Arduino.OpenConnection();
                resetActuators();
            }

            m_WhiteCube.Connect();
        }

        /// <summary>
        /// invoked when form is closing
        /// disconnect all open connections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IOTPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            //stop WMI event listeners
            m_Arduino.StopAutoReconnect();

            //unsubscribing, otherwise it will try to update the closed (non-existent form)
            m_Arduino.ArduinoConnectionStatusChange -= M_Arduino_ArduinoConnectionStatusChange;

            //close serial port connection
            m_Arduino.CloseConnection();

            //unsubscribing, otherwise it will try to update the closed (non-existent form)
            m_WhiteCube.WhiteCubeConnectionStatusChange -= M_WhiteCube_WhiteCubeConnectionStatusChange;

            //close MQTT connection
            m_WhiteCube.Disconnect();
        }

        #region connection notifications update labels
        /// <summary>
        /// invoked when White Cube Connection Status Change
        /// updates white cube connection label and color
        /// </summary>
        /// <param name="i_Status"></param>
        private void M_WhiteCube_WhiteCubeConnectionStatusChange(eWhiteCubeConnectionStatus i_Status)
        {
            if (this.InvokeRequired)
                this.Invoke(new WhiteCubeConnectionStatusChangeDelegate(M_WhiteCube_WhiteCubeConnectionStatusChange), i_Status);
            else
            {
                switch (i_Status)
                {
                    case eWhiteCubeConnectionStatus.Disconnected:
                        whiteCubeConnectionStateLabel.ForeColor = Color.Red;
                        pingSensorsButton.Enabled = false;
                        updateSensorsStatusButton.Enabled = false;
                        connectMQTTServerButton.Visible = true;
                        connectMQTTServerButton.Enabled = true;
                        break;
                    case eWhiteCubeConnectionStatus.Connected:
                        whiteCubeConnectionStateLabel.ForeColor = Color.Green;
                        pingSensorsButton.Enabled = true;
                        updateSensorsStatusButton.Enabled = true;
                        connectMQTTServerButton.Visible = false;
                        connectMQTTServerButton.Enabled = false;
                        break;
                }
                whiteCubeConnectionStateLabel.Text = i_Status.ToString();
            }
        }

        /// <summary>
        /// invoked when Arduino Connection Status Change
        /// updates arduino label and color
        /// </summary>
        /// <param name="i_Status"></param>
        private void M_Arduino_ArduinoConnectionStatusChange(eArduinoConnectionStatus i_Status)
        {
            if (this.InvokeRequired)
                this.Invoke(new ArduinoConnectionStatusChangeDelegate(M_Arduino_ArduinoConnectionStatusChange), i_Status);
            else
            {
                switch (i_Status)
                {
                    case eArduinoConnectionStatus.Detached:
                        arduinoConnectionStateLabel.ForeColor = Color.Red;
                        break;
                    case eArduinoConnectionStatus.Attached:
                        arduinoConnectionStateLabel.ForeColor = Color.Blue;
                        break;
                    case eArduinoConnectionStatus.Connected:
                        arduinoConnectionStateLabel.ForeColor = Color.Green;
                        resetActuators();
                        break;
                }
                arduinoConnectionStateLabel.Text = i_Status.ToString();
            }
        }
        #endregion

        /// <summary>
        /// Helper function that updates Control's text attribute immediately.
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

        #region Sensors status updaters
        /// <summary>
        /// invoked when DTH Sensor State Changes
        /// update relevant control with temp and humidity data
        /// </summary>
        /// <param name="i_Tempeprature"></param>
        /// <param name="i_Humidity"></param>
        private void M_WhiteCube_DTHSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            UpdateControlsText(DTHSensorTextBox, string.Format("Temp: {1}°C{0}Humid: {2}{0}{3}", /*{0}*/Environment.NewLine, /*{1}*/i_Tempeprature, /*{2}*/i_Humidity, /*{3}*/DateTime.Now.ToLongTimeString()));
        }

        /// <summary>
        /// invoked when Reed Sensor State Changes
        /// update relevant control
        /// </summary>
        /// <param name="i_Tempeprature"></param>
        /// <param name="i_Humidity"></param>
        private void M_WhiteCube_ReedSensorStateChange()
        {
            UpdateControlsText(ReedSensorTextBox , DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// invoked when Button clicked
        /// update relevant control
        /// </summary>
        /// <param name="i_Tempeprature"></param>
        /// <param name="i_Humidity"></param>
        private void M_WhiteCube_ButtonSensorStateChange()
        {
            UpdateControlsText(ButtonSensorTextBox, DateTime.Now.ToLongTimeString());
        }

        /// <summary>
        /// invoked when Light Sensor State Changes
        /// update relevant control with light value
        /// </summary>
        /// <param name="i_Tempeprature"></param>
        /// <param name="i_Humidity"></param>
        private void M_WhiteCube_LightSensorStateChange(int i_Value)
        {
            UpdateControlsText(LightSensorTextBox , string.Format("{1}{0}{2}", /*{0}*/Environment.NewLine, /*{1}*/i_Value.ToString(), /*{2}*/DateTime.Now.ToLongTimeString()));
        }
        #endregion

        #region Actuators status updaters
        /// <summary>
        /// invoked when Step Motor State Changes
        /// probably as a result of command issued by this panel
        /// </summary>
        /// <param name="i_State"></param>
        private void M_Arduino_StepMotorStateChange(eStepMotorState i_State)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new StepMotorStateChangeDelegate(M_Arduino_StepMotorStateChange), i_State);
            }
            else
            {
                UpdateControlsText(StepMotorTextBox, i_State.ToString());
                if (m_SmartChainActive)
                    StepMotorTextBox.BackColor = r_SmartChainBackColor;
            }
        }
        /// <summary>
        /// invoked when Servo Motor State Changes
        /// probably as a result of command issued by this panel
        /// </summary>
        /// <param name="i_State"></param>
        private void M_Arduino_ServoMotorStateChange(eServoMotorState i_State)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ServoMotorStateChangeDelegate(M_Arduino_ServoMotorStateChange), i_State);
            }
            else
            {
                UpdateControlsText(ServoMotorTextBox, i_State.ToString());
                if (m_SmartChainActive)
                    ServoMotorTextBox.BackColor = r_SmartChainBackColor;
            }
        }
        /// <summary>
        /// invoked when LED State Changes
        /// probably as a result of command issued by this panel
        /// </summary>
        /// <param name="i_State"></param>
        private void M_Arduino_LEDStateChange(eLEDState i_State)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LEDStateChangeDelegate(M_Arduino_LEDStateChange), i_State);
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
                    LEDTextBox.BackColor = r_SmartChainBackColor;
            }
        }
        /// <summary>
        /// invoked when RGB-LED State Changes
        /// probably as a result of command issued by this panel
        /// </summary>
        /// <param name="i_State"></param>
        private void M_Arduino_RGBLEDStateChange(eRGBLEDState i_State)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new RGBLEDStateChangeDelegate(M_Arduino_RGBLEDStateChange), i_State);
            }
            else
            {
                UpdateControlsText(RgbTextBox, i_State.ToString());
                if (m_SmartChainActive)
                {
                    RgbTextBox.BackColor = r_SmartChainBackColor;
                    endSmartChain();
                }
            }
        }
        #endregion

        #region Actuators buttons
        /// <summary>
        /// toggle, send command to LED to lit up when LED is off
        /// and send command to LED to turn off when it is lit up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// toggle Step Motor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StepMotorButton_Click(object sender, EventArgs e)
        {
            if (StepMotorTextBox.Text == "On")
                m_Arduino.SetStepMotor(eStepMotorState.Off);
            else if (StepMotorTextBox.Text == "Off")
                m_Arduino.SetStepMotor(eStepMotorState.On);
        }
        /// <summary>
        /// toggle Servo Motor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        #endregion

        #region Smart Chain Management
        /// <summary>
        /// toggle
        /// when off: start the chain reaction preperation:
        /// - disable the actuators group box and reset button
        /// - reset actuators
        /// - wait for actuators to be ready with timer (when they are all ready it will start)
        /// when on: stop the chain reaction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSmartChainButton_Click(object sender, EventArgs e)
        {
            if (startSmartChainButton.Text != "Stop") // start chain scenario
            {
                ActuatorsGroupBox.Enabled = false;
                resetButton.Enabled = false;
                resetActuators();
                m_ActuatorsReadyTimer.Start();
            }
            else
            {
                endSmartChain();
            }
        }

        /// <summary>
        /// if at least one of the actuators is not ready do not start the smart chain.
        /// otherwise (all actuators are turned off) stop this timer and start the smart chain
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// start smart chain (when all actuators are turned off (ready))
        /// - reset the smart chain timer
        /// - set the flag to true
        /// - subscribe to physical WhiteCube button click event
        /// - set the button text to stop
        /// </summary>
        private void startChainWhenActuatorsReady()
        {
            m_SmartChainRunTime = new TimeSpan(0, 0, 0);
            m_SmartChainActive = true;
            m_WhiteCube.ButtonSensorStateChange += SmartChainButton_Clicked; // smart chain timer starts here - when button is clicked
            startSmartChainButton.Text = "Stop";
        }

        /// <summary>
        /// every second add 1 second to counter and update runtime label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_SmartChainTimer_Tick(object sender, EventArgs e)
        {
            m_SmartChainRunTime = m_SmartChainRunTime.Add(new TimeSpan(0, 0, 1));
            RunTimeLabel.Text = m_SmartChainRunTime.ToString();
        }

        /// <summary>
        /// end smart chain:
        /// - set global flag to false
        /// - reset button text to "Start Smart Chain"
        /// - reset all text boxes colors
        /// - enable the reset button and the actuators group box
        /// </summary>
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
            resetButton.Enabled = true;
        }
        #endregion

        #region Smart Chain
        /// <summary>
        /// smart chain station 1
        /// invoked when whitecube physical button clicked
        /// - set text box color to yellow
        /// - start smart chain timer
        /// - unsubscribe (stop listening) to button clicks that affect smart chain
        /// - send command to turn on the led
        /// - subscribe to light sensor reading
        /// </summary>
        private void SmartChainButton_Clicked()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ButtonSensorStateChangeDelegate(SmartChainButton_Clicked));
            }
            else
            {
                ButtonSensorTextBox.BackColor = r_SmartChainBackColor;
                m_SmartChainTimer.Start();
                m_WhiteCube.ButtonSensorStateChange -= SmartChainButton_Clicked;
                m_Arduino.SetLED(eLEDState.On);
                m_WhiteCube.LightSensorStateChange += SmartChainLight_Sensed;
            }
        }

        /// <summary>
        /// smart chain station 3
        /// invoked when Light sensor read
        /// - set text box color to yellow
        /// - unsubscribe to light sensor readings
        /// - turn on step motor (falling domino blocks)
        /// - subscribe to reed sensor readings
        /// </summary>
        /// <param name="i_Value"></param>
        public void SmartChainLight_Sensed(int i_Value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new LightSensorStateChangeDelegate(SmartChainLight_Sensed), i_Value);
            }
            else
            {
                LightSensorTextBox.BackColor = r_SmartChainBackColor;
                m_WhiteCube.LightSensorStateChange -= SmartChainLight_Sensed;
                m_Arduino.SetStepMotor(eStepMotorState.On);
                m_WhiteCube.ReedSensorStateChange += SmartChainReedMagnet_Sensed;
            }
        }

        /// <summary>
        /// smart chain station 5
        /// invoked when Reed sensor read
        /// - set text box color to yellow
        /// - unsubscribe (stop listening) to Reed sensor readings that affect smart chain
        /// - turn off step motor
        /// - turn on servo motor
        /// - subscribe to DTH sensor reading
        /// </summary>
        public void SmartChainReedMagnet_Sensed()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ReedSensorStateChangeDelegate(SmartChainReedMagnet_Sensed));
            }
            else
            {
                ReedSensorTextBox.BackColor = r_SmartChainBackColor;
                m_WhiteCube.ReedSensorStateChange -= SmartChainReedMagnet_Sensed;
                m_Arduino.SetStepMotor(eStepMotorState.Off);
                m_Arduino.SetServoMotor(eServoMotorState.On);
                m_WhiteCube.DTHSensorStateChange += SmartChainDTH_Sensed;
            }
        }

        /// <summary>
        /// smart chain station 7
        /// invoked when DTH sensor read
        /// - set text box color to yellow
        /// - unsubscribe (stop listening) to DTH sensor readings that affect smart chain
        /// - set RGB led color according to DTH values received
        /// </summary>
        /// <param name="i_Temperature"></param>
        /// <param name="i_Humidity"></param>
        public void SmartChainDTH_Sensed(float i_Temperature, float i_Humidity)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DTHSensorStateChangeDelegate(SmartChainDTH_Sensed), i_Temperature, i_Humidity);
            }
            else
            {
                DTHSensorTextBox.BackColor = r_SmartChainBackColor;
                m_WhiteCube.DTHSensorStateChange -= SmartChainDTH_Sensed;
                if (i_Temperature + i_Humidity > 60)
                    m_Arduino.SetRGBLED(eRGBLEDState.Yellow);
                else if (i_Temperature + i_Humidity > 55)
                    m_Arduino.SetRGBLED(eRGBLEDState.Red);
                else if (i_Temperature + i_Humidity > 50)
                    m_Arduino.SetRGBLED(eRGBLEDState.Pink);
                else if (i_Temperature + i_Humidity > 45)
                    m_Arduino.SetRGBLED(eRGBLEDState.Green);
                else if (i_Temperature + i_Humidity > 40)
                    m_Arduino.SetRGBLED(eRGBLEDState.Azure);
                else if (i_Temperature + i_Humidity > 35)
                    m_Arduino.SetRGBLED(eRGBLEDState.Blue);
                else
                    m_Arduino.SetRGBLED(eRGBLEDState.White);
            }
        }
        #endregion

        /// <summary>
        /// Sending turn-off command to all actuators
        /// </summary>
        private void resetActuators()
        {
            if (LEDTextBox.Text != "Off")
                m_Arduino.SetLED(eLEDState.Off);
            if (ServoMotorTextBox.Text != "Off")
                m_Arduino.SetServoMotor(eServoMotorState.Off);
            if (StepMotorTextBox.Text != "Off")
                m_Arduino.SetStepMotor(eStepMotorState.Off);
            if (RgbTextBox.Text != "Off")
                m_Arduino.SetRGBLED(eRGBLEDState.Off);
        }

        /// <summary>
        /// reset all text boxes
        /// reset actuators
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            //reset all text boxes
            DTHSensorTextBox.Text    = String.Empty;
            LightSensorTextBox.Text  = String.Empty;
            ReedSensorTextBox.Text   = String.Empty;
            ButtonSensorTextBox.Text = String.Empty;
            
            //reset actuators
            resetActuators();
        }

        /// <summary>
        /// force white cube to identify all of its sensors immediatly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pingSensorsButton_Click(object sender, EventArgs e)
        {
            m_WhiteCube.SendMQTTCommand(eWhiteCubeCommand.Identify);
        }

        /// <summary>
        /// force white cube to update statuses from all of its sensors immediatly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateSensorsStatusButton_Click(object sender, EventArgs e)
        {
            m_WhiteCube.SendMQTTCommand(eWhiteCubeCommand.UpdateStatus);
        }

        private void connectMQTTServerButton_Click(object sender, EventArgs e)
        {
            m_WhiteCube.Connect();
        }
    }
}


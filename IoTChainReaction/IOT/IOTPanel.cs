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
        //bool on = true;
        bool toggleLight = true;
        Timer t = new Timer();
        private object textBox12;
        private object textBox13;
        private readonly object textBox14;
        private Arduino m_Arduino;
        private WhiteCube m_WhiteCube;

        public object TextBox9 { get; private set; }
        public object TextBox8 { get; private set; }

        public IOTPanel()
        {
            InitializeComponent();
            m_Arduino = new Arduino();
            m_Arduino.RGBLEDStateChange += M_Arduino_RGBLEDStateChange;
            m_Arduino.LEDStateChange += M_Arduino_LEDStateChange;
            m_Arduino.SetLED(eLEDState.Off);
            m_WhiteCube = new WhiteCube();
            m_WhiteCube.LightSensorStateChange += M_WhiteCube_LightSensorStateChange;
            m_WhiteCube.ButtonSensorStateChange += M_WhiteCube_ButtonSensorStateChange;
            m_WhiteCube.ReedSensorStateChange += M_WhiteCube_ReedSensorStateChange;
            m_WhiteCube.DTHSensorStateChange += M_WhiteCube_DTHSensorStateChange;
        }

        private void M_Arduino_LEDStateChange(eLEDState i_State)
        {
            if (LedButton.InvokeRequired)
            {
                LedButton.Invoke(new LEDStateChangeDelegate(M_Arduino_LEDStateChange), i_State);
            }
            else
            {
                UpdateTextBoxControl(textBox5, i_State.ToString());
                if (i_State == eLEDState.On)
                {
                    LedButton.BackColor = Color.Red;
                }
                else
                {
                    LedButton.BackColor = Color.DarkGray;
                }
            }
        }

        private void M_Arduino_RGBLEDStateChange(eRGBLEDState i_State)
        {
            UpdateTextBoxControl(RgbTextBox, i_State.ToString());
        }

        void UpdateTextBoxControl(Control ctrl, string str)
        {
            if(ctrl.InvokeRequired)
            {
                ctrl.Invoke(new UpdateTextBox(UpdateTextBoxControl), ctrl, str);
            }
            else
            {
                ctrl.Text = str;
            }
        }

        private void M_WhiteCube_DTHSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            UpdateTextBoxControl(TempSensorTextBox, "Temp:" + i_Tempeprature + "\r\n" + "Hum:" + i_Humidity);
        }

        private void M_WhiteCube_ReedSensorStateChange()
        {
            UpdateTextBoxControl(ReedSensorTextBox ,"DONE" + "\r\n" + DateTime.Now.ToLongTimeString());
        }

        private void M_WhiteCube_ButtonSensorStateChange()
        {
            UpdateTextBoxControl(ButtonSensorTextBox, DateTime.Now.ToLongTimeString());
        }

        private void M_WhiteCube_LightSensorStateChange(int i_Value)
        {
            UpdateTextBoxControl(LightSensorTextBox , i_Value.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /*private void button8_Click(object sender, EventArgs e)
        {
            
        }*/
        private void ButtunWBSensor_Click()
        {
            ButtonSensorTextBox.Text= "DONE"+"\r\n"+DateTime.Now.ToLongTimeString();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        private void reedWBSensor()
        {
            ReedSensorTextBox.Text="DONE"+"\r\n"+ DateTime.Now.ToLongTimeString(); 
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
           
            LedButton.Text = "LED";
            textBox5.Enabled = false;
            t.Interval = 1000;
            t.Tick += new EventHandler(t_Tick);
        }

        private void t_Tick(object sender, EventArgs e)
        {
            //This will perform a flashlight effect
            if(toggleLight)
            {
                LedButton.BackColor = Color.LimeGreen;
                toggleLight = false;
            }
            else
            {
                LedButton.BackColor = Color.Gray;
                toggleLight = true;
            }
        }

        

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
        //switch the button ON/OFF
            //ON
            if (textBox5.Text == "On")
            {
                m_Arduino.SetLED(eLEDState.Off);
                //t.Stop(); //stop the flashlight effect
            }
            else if ((textBox5.Text == "Off"))
            {
                m_Arduino.SetLED(eLEDState.On);
                //t.Start(); //start the flashlight effect
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ServoMotortextBox.Text == "ON")
                m_Arduino.SetServoMotor(eServoMotorState.Deg0);
            else if (ServoMotortextBox.Text == "OFF")
                m_Arduino.SetServoMotor(eServoMotorState.Deg180);
        }

      

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
             

        }

        /* private void button6_Click(object sender, EventArgs e)
         {

         }*/
        private void lightWBSensor_Click(int value)
        {
            LightSensorTextBox.Text = value + "\r\n" + DateTime.Now.ToLongTimeString();
        }

        /*private void button7_Click(object sender, EventArgs e)
         {

         }*/

        private void tempHumWBSensor_Click(double tempUpdate,double humidityUpdate) //event reciever
          {
            button7.Text = tempUpdate + "C" + "\r\n" + humidityUpdate + "%";
          }

        

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.R);
           
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.GB);
            
        }

        private void YellowButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.RG);
            

        }

        private void TurnOffButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.Off);
            
        }

        private void PinkButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.RB);
            RgbTextBox.Text = "Current Color: Pink";

        }

        private void BlueButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.B);
            RgbTextBox.Text = "Current Color: Blue";
        }

        private void WhiteButton_Click(object sender, EventArgs e)
        {
            m_Arduino.SetRGBLED(eRGBLEDState.RGB);
            RgbTextBox.Text = "Current Color: White";
        }
    }
}

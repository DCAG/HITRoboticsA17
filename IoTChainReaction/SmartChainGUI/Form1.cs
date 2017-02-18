using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using System.IO.Ports;
using System.Management;

namespace SmartChainLib
{
    public partial class Form1 : Form
    {
        //MqttClient m_WhiteCubeClient;

        public Form1()
        {
            InitializeComponent();
            //ConnectArduino();
            //ConnectWhiteCube();
        }



        public static string GenerateSessionID()
        {
            return Guid.NewGuid().ToString().Split('-')[4].Substring(0, 10);
        }

        private void Client_MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            SetValue(Encoding.UTF8.GetString(e.Message));
        }

        delegate void valueDelegate(string value);
        private void SetValue(string value)
        {
            if(textBox1.InvokeRequired)
            {
                textBox1.Invoke(new valueDelegate(SetValue), value);
            }
            else
            {
                textBox1.Text += value;
            }
        }

        private void Client_MqttMsgSubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs e)
        {
            SetValue(e.MessageId.ToString());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "clicked";
            actionsGroupBox.Enabled = !actionsGroupBox.Enabled;
            colorDialog1.ShowDialog();
            button1.Text = colorDialog1.Color.ToString();
        }
    }
}

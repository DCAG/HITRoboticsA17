using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;

namespace SmartChain
{
    public partial class Form1 : Form
    {
        MqttClient client;
        public Form1()
        {
            InitializeComponent();
            client = new MqttClient("139.162.222.115", 80, false, null, MqttSslProtocols.None,null,null);
            byte ConnResult = client.Connect(Guid.NewGuid().ToString().Split('-')[4].Substring(0, 10), "MATZI", "MATZI", true, 60);

            client.Subscribe(new string[] { "matzi/#" },new byte[] { 0 });
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            client.ToString();
        }

        private void Client_MqttMsgSubscribed(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgSubscribedEventArgs e)
        {
            //textBox1.Text += Environment.NewLine + e.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "clicked";
        }
    }
}

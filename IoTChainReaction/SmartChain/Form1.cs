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
        MqttClient mq;
        public Form1()
        {
            InitializeComponent();
            mq = new MqttClient():
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}

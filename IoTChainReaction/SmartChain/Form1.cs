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

namespace SmartChain
{
    public partial class Form1 : Form
    {
        MqttClient m_WhiteCubeClient;
        SerialPort m_ArduinoConnection;

        public Form1()
        {
            InitializeComponent();
            ConnectArduino();
            ConnectWhiteCube();
        }

        private void ConnectArduino()
        {
            m_ArduinoConnection = new SerialPort();
            m_ArduinoConnection.PortName = AutoDetectArduinoPort();
        }
        #region Arduino Port Auto Detection
        private void DeviceInsertedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void DeviceRemovedEvent(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            foreach (var property in instance.Properties)
            {
                Console.WriteLine(property.Name + " = " + property.Value);
            }
        }

        private void SubscribeToWMIInstances()
        {
            WqlEventQuery insertQuery = new WqlEventQuery("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher insertWatcher = new ManagementEventWatcher(insertQuery);
            insertWatcher.EventArrived += DeviceInsertedEvent;
            insertWatcher.Start();

            WqlEventQuery removeQuery = new WqlEventQuery("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub'");
            ManagementEventWatcher removeWatcher = new ManagementEventWatcher(removeQuery);
            removeWatcher.EventArrived += DeviceRemovedEvent;
            removeWatcher.Start();
        }

        private string AutoDetectArduinoPort()
        {

        }
        #endregion
        private void ConnectWhiteCube()
        {
            m_WhiteCubeClient = new MqttClient(Properties.Settings.Default.MQTTServerAddress,
                Properties.Settings.Default.MQTTServerPort, false, null, MqttSslProtocols.None, null, null);
            m_WhiteCubeClient.Subscribe(new string[] { "matzi/#" }, new byte[] { 0 });
            m_WhiteCubeClient.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            m_WhiteCubeClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            byte ConnResult = m_WhiteCubeClient.Connect(Guid.NewGuid().ToString().Split('-')[4].Substring(0, 10), "MATZI", "MATZI", true, 60);
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
        }
    }
}

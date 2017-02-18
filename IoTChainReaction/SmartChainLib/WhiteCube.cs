using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartChainLib
{
    public class WhiteCube : IWhiteCube
    {
        private string m_HostName;
        private int m_Port;
        private string m_UserName;
        private string m_Password;
        private ushort m_KeepAlivePeriod;

        MqttClient m_WhiteCubeClient;

        public event ButtonSensorStateChangeDelegate ButtonSensorStateChange;
        public event LightSensorStateChangeDelegate LightSensorStateChange;
        public event ReedSensorStateChangeDelegate ReedSensorStateChange;
        public event DHTSensorStateChangeDelegate DHTSensorStateChange;

        public WhiteCube()
        {
            m_HostName = Properties.Settings.Default.MQTTServerAddress;
            m_Port = Properties.Settings.Default.MQTTServerPort;
            m_UserName = Properties.Settings.Default.MQTTUserName;
            m_Password = Properties.Settings.Default.MQTTPassword;
            m_KeepAlivePeriod = Properties.Settings.Default.KeepAlivePeriod;
        }


        private void Connect()
        {
            const bool v_CleanSession = true;
            m_WhiteCubeClient = new MqttClient(Properties.Settings.Default.MQTTServerAddress,
                Properties.Settings.Default.MQTTServerPort, false, null, null, MqttSslProtocols.None, null);

            //"matzi/#"
            //"matzi/led/status"
            //"matzi/+/status"
            //"[username]/[device]/status"

            //"matzi/led/command", { "device_name":"3PI_8505689","";"","":1}
            m_WhiteCubeClient.Subscribe(new string[] { "matzi/#" }, new byte[] { 0 });
            m_WhiteCubeClient.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            m_WhiteCubeClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            byte ConnResult = m_WhiteCubeClient.Connect(GenerateSessionID(), Properties.Settings.Default.MQTTUserName, Properties.Settings.Default.MQTTPassword, v_CleanSession, Properties.Settings.Default.KeepAlivePeriod);

            /*
            Button:
                { "device_name":"3PI_1206876", "type":"button", "ipaddress":"192.168.8.246", "bgn":3, "uptime":58, "sdk":"1.4.0", "version":"0.2.1" }
                onclick:
                { "device_name":"3PI_1206876", "type":"button" }
                onrelease:
                { "device_name":"3PI_1206876", "type":"button" } X 2 (click and release)
             */

        }

        private string GenerateSessionID()
        {
            return Guid.NewGuid().ToString().Split('-')[4].Substring(0, 10);
        }

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void Send()
        {

        }

        void Recieve()
        {
            dynamic buttonJson = JsonConvert.SerializeObject("{ \"device_name\":\"3PI_1206876\", \"type\":\"button\" }");
            if(buttonJson.type == "button")
            {
                OnButtonSensorStateChange();
            }
            else if(buttonJson.type == "light")
            {
                OnLightSensorStateChange(buttonJson.value);
            }
            else if(buttonJson.type == "reed")
            {
                OnReedSensorStateChange();
            }
            else if(buttonJson.type == "dht")
            {
                OnDHTSensorStateChange(buttonJson.temp, buttonJson.humid);
            }
        }

        public void OnButtonSensorStateChange()
        {
            if(ButtonSensorStateChange != null)
            {
                ButtonSensorStateChange.Invoke();
            }
        }

        public void OnLightSensorStateChange(int i_Value)
        {
            if(LightSensorStateChange != null)
            {
                LightSensorStateChange.Invoke(i_Value);
            }
        }

        public void OnReedSensorStateChange()
        {
            if(ReedSensorStateChange != null)
            {
                ReedSensorStateChange.Invoke();
            }
        }

        public void OnDHTSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            if(DHTSensorStateChange != null)
            {
                DHTSensorStateChange.Invoke(i_Tempeprature, i_Humidity);
            }
        }

        public void RequestSensorStatus(eWhiteCubeSensor i_Sensor)
        {
            switch (i_Sensor)
            {
                case eWhiteCubeSensor.button:
                case eWhiteCubeSensor.reed:
                    //nothing
                    break;
                case eWhiteCubeSensor.dht:
                case eWhiteCubeSensor.light:
                    m_WhiteCubeClient.Publish(string.Format("{0}/{1}/status", m_UserName, i_Sensor),new byte[] { 0 });
                    break;
            }
        }
    }
}

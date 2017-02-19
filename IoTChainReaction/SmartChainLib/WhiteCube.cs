using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using Microsoft.CSharp;

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
        public event DTHSensorStateChangeDelegate DTHSensorStateChange;

        public WhiteCube()
        {
            m_HostName = Properties.Settings.Default.MQTTServerAddress;
            m_Port     = Properties.Settings.Default.MQTTServerPort;
            m_UserName = Properties.Settings.Default.MQTTUserName;
            m_Password = Properties.Settings.Default.MQTTPassword;
            m_KeepAlivePeriod = Properties.Settings.Default.KeepAlivePeriod;
            Connect();
        }


        private void Connect()
        {
            const bool v_CleanSession = true;
            const bool v_Secure = true;
            m_WhiteCubeClient = new MqttClient(m_HostName, m_Port, !v_Secure, null, null, MqttSslProtocols.None, null);

            string allMessages = string.Format("{0}/#", m_UserName.ToLower());
            byte qosLevel = 0;

            m_WhiteCubeClient.Subscribe(new string[] { allMessages }, new byte[] { qosLevel });
            m_WhiteCubeClient.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            m_WhiteCubeClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            byte ConnResult = m_WhiteCubeClient.Connect(GenerateSessionID(), m_UserName, m_Password, v_CleanSession, m_KeepAlivePeriod);

            /*
            Subscriptions:
            "matzi/#"
            "matzi/led/status"
            "matzi/+/status"
            "[username]/[device]/status"

            Commands:
            "matzi/led/command", { "device_name":"3PI_8505689","";"","":1}
            
            Examples:
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
            try
            {
                dynamic messageJson = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Message)); //("{ \"device_name\":\"3PI_1206876\", \"type\":\"button\" }");
                if (messageJson.ipaddress != null)
                    return; // { "device_name":"3PI_1206876", "type":"button", "ipaddress":"192.168.8.246", "bgn":3, "uptime":21, "sdk":"1.4.0", "version":"0.2.1" }
                if ((string)messageJson.type == eWhiteCubeSensor.button.ToString())
                {
                    OnButtonSensorStateChange();
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.light.ToString())
                {
                    OnLightSensorStateChange((int)messageJson.value); //{ "device_name":"3PI_1152554", "type":"light", "value":"185" }
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.reed.ToString())
                {
                    OnReedSensorStateChange();
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.dth.ToString())
                {
                    OnDTHSensorStateChange((float)messageJson.temperature, (float)messageJson.humidity); //{ "device_name":"3PI_13775673", "type":"dth", "temperature":"20.0", "humidity":"20.0" }
                }
            }
            catch(Exception ex)
            {
                //Console.WriteLine(ex.Message);//Encoding.UTF8.GetString(e.Message));//alert of wrong incoming message - where to?
            }
        }

        private void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            //Console.WriteLine("{0}\nQoS:{1}, MsgId:{2}",e, Encoding.UTF8.GetString(e.GrantedQoSLevels) ,e.MessageId);//alert of wrong incoming message - where to?
        }

        private void OnButtonSensorStateChange()
        {
            if(ButtonSensorStateChange != null)
            {
                ButtonSensorStateChange.Invoke();
            }
        }

        private void OnLightSensorStateChange(int i_Value)
        {
            if(LightSensorStateChange != null)
            {
                LightSensorStateChange.Invoke(i_Value);
            }
        }

        private void OnReedSensorStateChange()
        {
            if(ReedSensorStateChange != null)
            {
                ReedSensorStateChange.Invoke();
            }
        }

        private void OnDTHSensorStateChange(float i_Tempeprature, float i_Humidity)
        {
            if(DTHSensorStateChange != null)
            {
                DTHSensorStateChange.Invoke(i_Tempeprature, i_Humidity);
            }
        }

        public void RequestSensorStatus(eWhiteCubeSensor i_Sensor)
        {
            switch (i_Sensor)
            {
                case eWhiteCubeSensor.button:
                case eWhiteCubeSensor.reed:
                    //do not send ahything - when these sensors detect something they will send a message.
                    break;
                case eWhiteCubeSensor.dth:
                case eWhiteCubeSensor.light:
                    m_WhiteCubeClient.Publish(string.Format("{0}/{1}/status", m_UserName, i_Sensor),new byte[] { 0 });
                    break;
            }
        }
    }
}

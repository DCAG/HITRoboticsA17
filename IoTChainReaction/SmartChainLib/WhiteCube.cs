using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using Newtonsoft.Json;
using System.Timers;

namespace SmartChainLib
{
    public delegate void WhiteCubeConnectionStatusChangeDelegate(eWhiteCubeConnectionStatus i_Status);
    public delegate void ButtonSensorStateChangeDelegate();
    public delegate void LightSensorStateChangeDelegate(int i_Value);
    public delegate void ReedSensorStateChangeDelegate();
    public delegate void DTHSensorStateChangeDelegate(float i_Tempeprature, float i_Humidity);

    public class WhiteCube
    {
        private string m_HostName;
        private int m_Port;
        private string m_UserName;
        private string m_Password;
        private ushort m_KeepAlivePeriod;

        MqttClient m_WhiteCubeClient;

        public event WhiteCubeConnectionStatusChangeDelegate WhiteCubeConnectionStatusChange;
        public event ButtonSensorStateChangeDelegate ButtonSensorStateChange;
        public event LightSensorStateChangeDelegate LightSensorStateChange;
        public event ReedSensorStateChangeDelegate ReedSensorStateChange;
        public event DTHSensorStateChangeDelegate DTHSensorStateChange;

        /// <summary>
        /// 
        /// </summary>
        public WhiteCube()
        {
            m_HostName = Properties.Settings.Default.MQTTServerAddress;
            m_Port     = Properties.Settings.Default.MQTTServerPort;
            m_UserName = Properties.Settings.Default.MQTTUserName;
            m_Password = Properties.Settings.Default.MQTTPassword;
            m_KeepAlivePeriod = Properties.Settings.Default.KeepAlivePeriod;

            const bool v_Secure = true;
            m_WhiteCubeClient = new MqttClient(m_HostName, m_Port, !v_Secure, null, null, MqttSslProtocols.None, null);

            const byte qosLevel = 0;
            string allMessages = string.Format("{0}/#", m_UserName.ToLower());
            m_WhiteCubeClient.Subscribe(new string[] { allMessages }, new byte[] { qosLevel });
            m_WhiteCubeClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            m_WhiteCubeClient.ConnectionClosed += M_WhiteCubeClient_ConnectionClosed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void M_WhiteCubeClient_ConnectionClosed(object sender, EventArgs e)
        {
            OnWhiteCubeConnectionStatusChange(eWhiteCubeConnectionStatus.Disconnected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Status"></param>
        private void OnWhiteCubeConnectionStatusChange(eWhiteCubeConnectionStatus i_Status)
        {
            if(WhiteCubeConnectionStatusChange != null)
            {
                WhiteCubeConnectionStatusChange.Invoke(i_Status);
            }
        }

        /// <summary>
        /// Subscriptions:
        /// "matzi/#"
        /// "matzi/led/status"
        /// "matzi/+/status"
        /// "[username]/[device]/status"
        ///
        /// Commands:
        /// "matzi/led/command", { "device_name":"3PI_8505689","";"","":1}
        /// 
        /// Examples:
        /// Button:
        ///     { "device_name":"3PI_1206876", "type":"button", "ipaddress":"192.168.8.246", "bgn":3, "uptime":58, "sdk":"1.4.0", "version":"0.2.1" }
        ///     onclick:
        ///     { "device_name":"3PI_1206876", "type":"button" }
        /// </summary>
        public void Connect()
        {
            const bool v_CleanSession = true;

            if (!m_WhiteCubeClient.IsConnected)
            {
                try
                {
                    m_WhiteCubeClient.Connect(GenerateSessionID(), m_UserName, m_Password, v_CleanSession, m_KeepAlivePeriod);
                    OnWhiteCubeConnectionStatusChange(eWhiteCubeConnectionStatus.Connected);
                }
                catch
                {
                    // log or report...
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            if(m_WhiteCubeClient.IsConnected)
            {
                m_WhiteCubeClient.Disconnect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GenerateSessionID()
        {
            return Guid.NewGuid().ToString().Split('-')[4].Substring(0, 10);
        }

        /// <summary>
        ///
        /// Ignore EXISTENCE notification messages like this one
        ///  {
        ///      "device_name" : "3PI_1206876",
        ///      "type" : "button",
        ///      "ipaddress" : "192.168.8.246",
        ///      "bgn" : 3,
        ///      "uptime" : 21,
        ///      "sdk" : "1.4.0",
        ///      "version" : "0.2.1"
        ///  }
        ///
        /// Accept EVENT messsages like this one:
        ///  {
        ///      "device_name" : "3PI_1206876",
        ///      "type" : "button"
        ///  }
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                dynamic messageJson = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Message));

                if (messageJson.ipaddress != null)
                    return; 
                if ((string)messageJson.type == eWhiteCubeSensor.button.ToString())
                {
                    OnButtonSensorStateChange();
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.light.ToString())
                {
                    OnLightSensorStateChange((int)messageJson.value); 
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.reed.ToString())
                {
                    OnReedSensorStateChange();
                }
                else if ((string)messageJson.type == eWhiteCubeSensor.dth.ToString())
                {
                    OnDTHSensorStateChange((float)messageJson.temperature, (float)messageJson.humidity); 
                }
            }
            catch
            {
                // write to log (debug?) - maybe convertion failed or format is not json
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnButtonSensorStateChange()
        {
            if(ButtonSensorStateChange != null)
            {
                ButtonSensorStateChange.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Value"></param>
        private void OnLightSensorStateChange(int i_Value)
        {
            if(LightSensorStateChange != null)
            {
                LightSensorStateChange.Invoke(i_Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnReedSensorStateChange()
        {
            if(ReedSensorStateChange != null)
            {
                ReedSensorStateChange.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i_Tempeprature"></param>
        /// <param name="i_Humidity"></param>
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

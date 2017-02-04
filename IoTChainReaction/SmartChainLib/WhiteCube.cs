using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace SmartChainLib
{
    public class WhiteCube
    {
        private string m_HostName;
        private int m_Port;
        private string m_UserName;
        private string m_Password;
        private ushort m_KeepAlivePeriod;

        MqttClient m_WhiteCubeClient;

        WhiteCube()
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

            m_WhiteCubeClient.Subscribe(new string[] { "matzi/#" }, new byte[] { 0 });
            m_WhiteCubeClient.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            m_WhiteCubeClient.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            byte ConnResult = m_WhiteCubeClient.Connect(GenerateSessionID(), Properties.Settings.Default.MQTTUserName, Properties.Settings.Default.MQTTPassword, v_CleanSession, Properties.Settings.Default.KeepAlivePeriod);
        }

        void Send()
        {

        }

        void Recieve()
        {


        }
    }
}

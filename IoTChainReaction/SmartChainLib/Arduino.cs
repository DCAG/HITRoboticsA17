using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SmartChainLib
{
    public class Arduino
    {
        private string m_Port;
        private string m_VID;
        private string m_PID;

        public event EventArrivedEventHandler OnDeviceArrival;
        public event EventArrivedEventHandler OnDeviceRemoval;

        SerialPort m_ArduinoConnection;

        public Arduino()
        {
            m_VID = Properties.Settings.Default.ArduinoPID;
            m_PID = Properties.Settings.Default.ArduinoVID;
            m_Port = Properties.Settings.Default.ArduinoCOMPort;
        }

        public void WriteLine(string message)
        {
            if (m_ArduinoConnection == null)
            {
                m_ArduinoConnection = new SerialPort(m_Port);
                m_ArduinoConnection.DataReceived += M_ArduinoConnection_DataReceived;
            }
            if(!m_ArduinoConnection.IsOpen)
            {
                m_ArduinoConnection.Open();
            }
            m_ArduinoConnection.WriteLine(message);
        }

        private void M_ArduinoConnection_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            m_ArduinoConnection.ReadLine(); ///WHERE the DATA GOES!!!!@!?@?!?@!?!@????????
        }

        public void AutoDetectArduinoPort()
        {
            subscribeToWMIInstances();
            OnDeviceArrival += OnArduinoArrival;
            OnDeviceRemoval += OnArduinoRemoval;
        }

        private void subscribeToWMIInstances()
        {
            string deviceArrivalQuery = string.Format("SELECT * FROM __InstanceCreationEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub' AND TargetInstance.DeviceID LIKE '%VID_{0}`&PID_{1}%'", m_VID, m_PID);
            ManagementEventWatcher arrivalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceArrivalQuery));
            arrivalWatcher.EventArrived += OnDeviceArrival;
            arrivalWatcher.Start();

            string deviceRemovalQuery = string.Format("SELECT * FROM __InstanceDeletionEvent WITHIN 2 WHERE TargetInstance ISA 'Win32_USBHub' AND TargetInstance.DeviceID LIKE '%VID_{0}`&PID_{1}%'", m_VID, m_PID);
            ManagementEventWatcher removalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceRemovalQuery));
            removalWatcher.EventArrived += OnDeviceRemoval;
            removalWatcher.Start();
        }

        private void OnArduinoArrival(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            m_Port = (string)instance["COMPort"];
        }

        private void OnArduinoRemoval(object sender, EventArrivedEventArgs e)
        {
            m_Port = string.Empty;
        }
    }
}

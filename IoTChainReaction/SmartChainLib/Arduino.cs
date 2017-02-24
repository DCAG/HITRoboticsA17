using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartChainLib
{
    public delegate void ArduinoConnectionStatusChangeDelegate(eArduinoConnectionStatus i_Status);
    public delegate void LEDStateChangeDelegate(eLEDState i_State);
    public delegate void ServoMotorStateChangeDelegate(eServoMotorState i_State);
    public delegate void StepMotorStateChangeDelegate(eStepMotorState i_State);
    public delegate void RGBLEDStateChangeDelegate(eRGBLEDState i_State);

    public class Arduino : IArduino
    {
        SerialPort m_ArduinoConnection;

        private bool m_IsConnectedToComputer;
        public bool IsConnectedToComputer
        {
            get
            {
                return m_IsConnectedToComputer;
            }
        }

        private string m_Port;
        private string m_PNPDeviceName;
        
        public event ArduinoConnectionStatusChangeDelegate ArduinoConnectionStatusChange;
        public event LEDStateChangeDelegate LEDStateChange;
        public event ServoMotorStateChangeDelegate ServoMotorStateChange;
        public event StepMotorStateChangeDelegate StepMotorStateChange;
        public event RGBLEDStateChangeDelegate RGBLEDStateChange;

        private ManagementEventWatcher m_ArrivalWatcher;
        private ManagementEventWatcher m_RemovalWatcher;

        public Arduino()
        {
            m_PNPDeviceName = Properties.Settings.Default.ArduinoPNPDeviceName;
            m_Port = GetArduinoComPort(m_PNPDeviceName);
            m_IsConnectedToComputer = m_Port != string.Empty;
            OnArduinoConnectionStatusChange(m_IsConnectedToComputer ? eArduinoConnectionStatus.Attached : eArduinoConnectionStatus.Detached);
            m_ArduinoConnection = new SerialPort();
            m_ArduinoConnection.DataReceived += M_ArduinoConnection_DataReceived;

            string serialDeviceQuery = "SELECT * FROM {0} WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort' AND TargetInstance.Name LIKE '%{1}%'";
            string deviceArrivalQuery = string.Format(serialDeviceQuery, "__InstanceCreationEvent", m_PNPDeviceName);
            m_ArrivalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceArrivalQuery));
            m_ArrivalWatcher.EventArrived += OnArrival;
            string deviceRemovalQuery = string.Format(serialDeviceQuery, "__InstanceDeletionEvent", m_PNPDeviceName);
            m_RemovalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceRemovalQuery));
            m_RemovalWatcher.EventArrived += OnRemoval;
        }

        public void OpenConnection()
        {
            m_ArduinoConnection.PortName = m_Port;
            if (!m_ArduinoConnection.IsOpen)
            {
                try
                {
                    m_ArduinoConnection.Open();
                    OnArduinoConnectionStatusChange(eArduinoConnectionStatus.Connected);
                }
                catch
                {
                    // write to log (debug?)
                }
            }
        }

        public void CloseConnection()
        {
            if (m_ArduinoConnection.IsOpen)
            {
                try
                {
                    m_ArduinoConnection.Close();
                    OnArduinoConnectionStatusChange(eArduinoConnectionStatus.Attached);
                }
                catch
                {
                    // write to log (debug?)
                }
            }
        }

        public void WriteLine(string message)
        {
            m_ArduinoConnection.WriteLine(message + "\n");
        }

        private void M_ArduinoConnection_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = m_ArduinoConnection.ReadLine();

            Regex regex = new Regex(@"^A(?<actuator>L|S|M|R)(?<state>\d)");
            Match result = regex.Match(data);
            if (result.Success)
            {
                string actuator = result.Groups["actuator"].Value;
                int state = int.Parse(result.Groups["state"].Value);

                if (actuator == "L")
                {
                    OnLEDStateChange((eLEDState)state);
                }
                else if (actuator == "S")
                {
                    OnServoMotorStateChange((eServoMotorState)state);
                }
                else if (actuator == "M")
                {
                    OnStepMotorStateChange((eStepMotorState)state);
                }
                else if (actuator == "R")
                {
                    OnRGBLEDStateChange((eRGBLEDState)state);
                }
            }
        }

        #region Autodetect Arduino connection
        private void OnArduinoConnectionStatusChange(eArduinoConnectionStatus i_Status)
        {
            if(ArduinoConnectionStatusChange != null)
            {
                ArduinoConnectionStatusChange.Invoke(i_Status);
            }
        }

        public static string GetArduinoComPort(string i_DeviceName)
        {
            string arduinoDeviceQuery = string.Format("SELECT * FROM Win32_SerialPort WHERE Name LIKE '%{0}%'", i_DeviceName);
            
            ObjectQuery query = new ObjectQuery(arduinoDeviceQuery); 
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection serialCOMDevices = searcher.Get();
            ManagementObject arduinoMgmtObject = serialCOMDevices.Cast<ManagementObject>().ToList().FirstOrDefault() ?? null;

            string result;
            if (arduinoMgmtObject == null)
            {
                result = string.Empty;
            }
            else
            {
                result = arduinoMgmtObject["DeviceID"].ToString();
            }

            return result;
        }

        public void StartSubscribingToDeviceAttachAutoConnectAndDetachAutoClose()
        {
            m_ArrivalWatcher.Start();
            m_RemovalWatcher.Start();
        }

        public void StopSubscribingToDeviceAttachAutoConnectAndDetachAutoClose()
        {
            m_ArrivalWatcher.Stop();
            m_RemovalWatcher.Stop();
        }

        private void OnArrival(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            m_Port = (string)instance["DeviceID"];

            m_IsConnectedToComputer = true;
            OnArduinoConnectionStatusChange(eArduinoConnectionStatus.Attached);
            OpenConnection();
        }

        private void OnRemoval(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Arrival m_Port =  {0}", m_Port);
            m_Port = string.Empty;

            m_IsConnectedToComputer = false;
            CloseConnection();
            OnArduinoConnectionStatusChange(eArduinoConnectionStatus.Detached);
        }
        #endregion

        #region Sensors And Actuators
        public void SetLED(eLEDState i_State)
        {
            WriteLine(string.Format("AL{0}", (int)i_State));
        }

        public void SetServoMotor(eServoMotorState i_State)
        {
            WriteLine(string.Format("AS{0}", (int)i_State));
        }

        public void SetStepMotor(eStepMotorState i_State)
        {
            WriteLine(string.Format("AM{0}", (int)i_State));
        }

        public void SetRGBLED(eRGBLEDState i_State)
        {
            WriteLine(string.Format("AR{0}", (int)i_State));
        }

        private void OnLEDStateChange(eLEDState i_State)
        {
            if(LEDStateChange != null)
            {
                LEDStateChange.Invoke(i_State);
            }
        }

        private void OnServoMotorStateChange(eServoMotorState i_State)
        {
            if (ServoMotorStateChange != null)
            {
                ServoMotorStateChange.Invoke(i_State);
            }
        }

        private void OnStepMotorStateChange(eStepMotorState i_State)
        {
            if (StepMotorStateChange != null)
            {
                StepMotorStateChange.Invoke(i_State);
            }
        }

        private void OnRGBLEDStateChange(eRGBLEDState i_State)
        {
            if (RGBLEDStateChange != null)
            {
                RGBLEDStateChange.Invoke(i_State);
            }
        }
        #endregion
    }
}

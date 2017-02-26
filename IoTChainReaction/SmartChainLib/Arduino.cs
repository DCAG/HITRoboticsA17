using System;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace SmartChainLib
{
    public delegate void ArduinoConnectionStatusChangeDelegate(eArduinoConnectionStatus i_Status);
    public delegate void LEDStateChangeDelegate(eLEDState i_State);
    public delegate void ServoMotorStateChangeDelegate(eServoMotorState i_State);
    public delegate void StepMotorStateChangeDelegate(eStepMotorState i_State);
    public delegate void RGBLEDStateChangeDelegate(eRGBLEDState i_State);

    public class Arduino
    {
        SerialPort m_ArduinoConnection;
        private string m_Port;
        private string m_PNPDeviceName; //Plug and Play Device Name
        private bool m_IsConnectedToComputer;
        public bool IsConnectedToComputer
        {
            get
            {
                return m_IsConnectedToComputer;
            }
        }

        private ManagementEventWatcher m_ArrivalWatcher;
        private ManagementEventWatcher m_RemovalWatcher;

        public event ArduinoConnectionStatusChangeDelegate ArduinoConnectionStatusChange;
        public event LEDStateChangeDelegate LEDStateChange;
        public event ServoMotorStateChangeDelegate ServoMotorStateChange;
        public event StepMotorStateChangeDelegate StepMotorStateChange;
        public event RGBLEDStateChangeDelegate RGBLEDStateChange;

        /// <summary>
        /// 
        /// </summary>
        public Arduino()
        {
            /*
             * Get Arduino mapped serial port (COM#)
             */ 
            m_PNPDeviceName = Properties.Settings.Default.ArduinoPNPDeviceName;
            m_Port = GetArduinoComPort(m_PNPDeviceName);
            m_IsConnectedToComputer = m_Port != string.Empty;
            OnArduinoConnectionStatusChange(m_IsConnectedToComputer ? eArduinoConnectionStatus.Attached : eArduinoConnectionStatus.Detached);

            /*
             * initialize arduino serialport object and notifications:
             */
            m_ArduinoConnection = new SerialPort();
            m_ArduinoConnection.DataReceived += M_ArduinoConnection_DataReceived;

            /*
             * auto detect arrival/removal via WMI events notification - initialization of notification and query objects
             */
            string serialDeviceQuery = "SELECT * FROM {0} WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort' AND TargetInstance.Name LIKE '%{1}%'";
            string deviceArrivalQuery = string.Format(serialDeviceQuery, "__InstanceCreationEvent", m_PNPDeviceName);
            m_ArrivalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceArrivalQuery));
            m_ArrivalWatcher.EventArrived += OnArrival;
            string deviceRemovalQuery = string.Format(serialDeviceQuery, "__InstanceDeletionEvent", m_PNPDeviceName);
            m_RemovalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceRemovalQuery));
            m_RemovalWatcher.EventArrived += OnRemoval;
        }

        /// <summary>
        /// Open the serial port connection to the arduino device
        /// </summary>
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

        /// <summary>
        /// Close the serial port connection to the arduino device
        /// </summary>
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

        /// <summary>
        /// Send a message to the arduino device via the open serial connection.
        /// </summary>
        /// <param name="message">message to send to the arduino device</param>
        public void WriteLine(string message)
        {
            if(IsConnectedToComputer)
                m_ArduinoConnection.WriteLine(message + "\n");
        }

        /// <summary>
        /// invoked when the arduino device sent a message and it is waiting to be read.
        /// reads the message, parse it with regular expression
        /// and notify subscribers that something has happened (actuator state was changed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// notify that arduino connection state was chaned
        /// </summary>
        /// <param name="i_Status"></param>
        private void OnArduinoConnectionStatusChange(eArduinoConnectionStatus i_Status)
        {
            if(ArduinoConnectionStatusChange != null)
            {
                ArduinoConnectionStatusChange.Invoke(i_Status);
            }
        }

        /// <summary>
        /// Query WMI class win32_SerialPort for the COM port number that was mapped to the arduino device.
        /// This is a static function therefore has i_DeviceName string parameter.
        /// </summary>
        /// <param name="i_DeviceName">The name of the device to include in the query for the COM port from the Win32_SerialPort WMI class</param>
        /// <returns>COM port number as string e.g. 'COM4', if device was not found empty string is returned.</returns>
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

        /// <summary>
        /// Start listening to WMI arriavl and removal events for win32_SerialPort devices
        /// when device arrives, initiates new connection.
        /// </summary>
        public void StartAutoReconnect()
        {
            m_ArrivalWatcher.Start();
            m_RemovalWatcher.Start();
        }

        /// <summary>
        /// Stop listening to WMI arriavl and removal events for win32_SerialPort devices
        /// </summary>
        public void StopAutoReconnect()
        {
            m_ArrivalWatcher.Stop();
            m_RemovalWatcher.Stop();
        }

        /// <summary>
        /// invoked when arduino device is attached to the computer.
        /// extracts the COM port number
        /// open the connection
        /// and alert all subscribers that the arduino device is present
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnArrival(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            m_Port = (string)instance["DeviceID"];

            m_IsConnectedToComputer = true;
            OnArduinoConnectionStatusChange(eArduinoConnectionStatus.Attached);
            OpenConnection();
        }

        /// <summary>
        /// invoked when arduino device is detached from the computer.
        /// erase the COM port number
        /// close the connection
        /// and alert all subscribers that the arduino device is absent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Send command to the arduino device to set the LED state
        /// </summary>
        /// <param name="i_State">desired LED state</param>
        public void SetLED(eLEDState i_State)
        {
            WriteLine(string.Format("AL{0}", (int)i_State));
        }

        /// <summary>
        /// Send command to the arduino device to set the Servo-Motor state
        /// </summary>
        /// <param name="i_State">desired Servo-Motor state</param>
        public void SetServoMotor(eServoMotorState i_State)
        {
            WriteLine(string.Format("AS{0}", (int)i_State));
        }

        /// <summary>
        /// Send command to the arduino device to set the Step-Motor state
        /// </summary>
        /// <param name="i_State">desired Step-Motor state</param>
        public void SetStepMotor(eStepMotorState i_State)
        {
            WriteLine(string.Format("AM{0}", (int)i_State));
        }

        /// <summary>
        /// Send command to the arduino device to set the RGB-LED state
        /// </summary>
        /// <param name="i_State">desired RGB-LED state</param>
        public void SetRGBLED(eRGBLEDState i_State)
        {
            WriteLine(string.Format("AR{0}", (int)i_State));
        }

        /// <summary>
        /// Notify all 'LEDStateChange' event subscribers that the LED state was changed
        /// </summary>
        /// <param name="i_State"></param>
        private void OnLEDStateChange(eLEDState i_State)
        {
            if(LEDStateChange != null)
            {
                LEDStateChange.Invoke(i_State);
            }
        }

        /// <summary>
        /// Notify all 'ServoMotorStateChange' event subscribers that the Servo-Motor state was changed
        /// </summary>
        /// <param name="i_State"></param>
        private void OnServoMotorStateChange(eServoMotorState i_State)
        {
            if (ServoMotorStateChange != null)
            {
                ServoMotorStateChange.Invoke(i_State);
            }
        }

        /// <summary>
        /// Notify all 'StepMotorStateChange' event subscribers that the Step-Motor state was changed
        /// </summary>
        /// <param name="i_State"></param>
        private void OnStepMotorStateChange(eStepMotorState i_State)
        {
            if (StepMotorStateChange != null)
            {
                StepMotorStateChange.Invoke(i_State);
            }
        }

        /// <summary>
        /// Notify all 'RGBLEDStateChange' event subscribers that the RGB-LED state was changed
        /// </summary>
        /// <param name="i_State"></param>
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

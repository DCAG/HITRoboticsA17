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
        private string m_VID;
        private string m_PID;

        //public event EventArrivedEventHandler OnDeviceArrival;
        //public event EventArrivedEventHandler OnDeviceRemoval;

        public event LEDStateChangeDelegate LEDStateChange;
        public event ServoMotorStateChangeDelegate ServoMotorStateChange;
        public event StepMotorStateChangeDelegate StepMotorStateChange;
        public event RGBLEDStateChangeDelegate RGBLEDStateChange;

        public Arduino()
        {
            m_IsConnectedToComputer = false;
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
            m_ArduinoConnection.WriteLine(message+"\n");
        }

        private void M_ArduinoConnection_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = m_ArduinoConnection.ReadLine();
            
            Regex regex = new Regex(@"^A(?<actuator>L|S|M|R)(?<state>\d)");
            Match result = regex.Match(data);
            if(result.Success)
            {
                string actuator = result.Groups["actuator"].Value;
                int state = int.Parse(result.Groups["state"].Value);

                if (actuator == "L")
                {
                    OnLEDStateChange((eLEDState)state);
                }
                else if(actuator == "S")
                {
                    OnServoMotorStateChange((eServoMotorState)state);
                }
                else if(actuator == "M")
                {
                    OnStepMotorStateChange((eStepMotorState)state);
                }
                else if(actuator == "R")
                {
                    OnRGBLEDStateChange((eRGBLEDState)state);
                }
            }
        }

        #region Autodetect Arduino connection
        public void AutoDetectArduinoPort()
        {
            //OnDeviceArrival += OnArduinoArrival;
            //OnDeviceRemoval += OnArduinoRemoval;
            subscribeToWMIInstances();
        }

        private void subscribeToWMIInstances()
        {
            string serialDeviceQuery = "SELECT * FROM {0} WITHIN 2 WHERE TargetInstance ISA 'Win32_SerialPort' AND TargetInstance.PNPDeviceID LIKE '%VID_{1}&PID_{2}%'";

            string deviceArrivalQuery = string.Format(serialDeviceQuery, "__InstanceCreationEvent", m_VID, m_PID);
            ManagementEventWatcher arrivalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceArrivalQuery));
            arrivalWatcher.EventArrived += OnArduinoArrival; //OnDeviceArrival;
            arrivalWatcher.Start();

            string deviceRemovalQuery = string.Format(serialDeviceQuery, "__InstanceDeletionEvent", m_VID, m_PID);
            ManagementEventWatcher removalWatcher = new ManagementEventWatcher(new WqlEventQuery(deviceRemovalQuery));
            removalWatcher.EventArrived += OnArduinoRemoval;  //OnDeviceRemoval;
            removalWatcher.Start();
        }

        private void OnArduinoArrival(object sender, EventArrivedEventArgs e)
        {
            ManagementBaseObject instance = (ManagementBaseObject)e.NewEvent["TargetInstance"];
            m_Port = (string)instance["COMPort"];
            Console.WriteLine("Arrival m_Port =  {0}", m_Port);
            WriteLine("1Hello");
            WriteLine("2World");
            WriteLine("3Arduino repeat the time");
            WriteLine("4" + DateTime.Now.ToString());
            WriteLine("5Hello");
            WriteLine("6Hello");
            WriteLine("7Hello");
            WriteLine("9Hello");
        }

        private void OnArduinoRemoval(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Arrival m_Port =  {0}", m_Port);
            m_Port = string.Empty;
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

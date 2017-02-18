using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartChainLib
{
    public delegate void ButtonSensorStateChangeDelegate();
    public delegate void LightSensorStateChangeDelegate(int i_Value);
    public delegate void ReedSensorStateChangeDelegate();
    public delegate void DHTSensorStateChangeDelegate(float i_Tempeprature, float i_Humidity);
    public interface IWhiteCube
    {
        void RequestSensorStatus(eWhiteCubeSensor i_Sensor);

        event ButtonSensorStateChangeDelegate ButtonSensorStateChange;
        event LightSensorStateChangeDelegate LightSensorStateChange;
        event ReedSensorStateChangeDelegate ReedSensorStateChange;
        event DHTSensorStateChangeDelegate DHTSensorStateChange;

        void OnButtonSensorStateChange();
        void OnLightSensorStateChange(int i_Value);
        void OnReedSensorStateChange();
        void OnDHTSensorStateChange(float i_Tempeprature, float i_Humidity);
    }
}

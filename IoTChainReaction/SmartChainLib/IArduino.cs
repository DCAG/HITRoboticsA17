using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartChainLib
{


    interface IArduino
    {
        void SetLED(eLEDState i_State);

        void SetServoMotor(eServoMotorState i_State);

        void SetStepMotor(eStepMotorState i_State);

        void SetRGBLED(eRGBLEDState i_State);

        event LEDStateChangeDelegate LEDStateChange;
        event ServoMotorStateChangeDelegate ServoMotorStateChange;
        event StepMotorStateChangeDelegate StepMotorStateChange;
        event RGBLEDStateChangeDelegate RGBLEDStateChange;

        /*
        void OnLEDStateChange(eLEDState i_State);
        void OnServoMotorStateChange(eServoMotorState i_State);
        void OnStepMotorStateChange(eStepMotorState i_State);
        void OnRGBLEDStateChange(eRGBLEDState i_State);
        */
    }
}

namespace AutomatedCar.SystemComponents.CruiseControl
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CruiseControl : SystemComponent
    {
        public bool ACCenabled;

        public CruiseControl(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.ACCenabled = false;
        }

        public override void Process()
        {
            //throw new NotImplementedException();
            switch (this.virtualFunctionBus.InputPacket.CruiseControlInput)
            {
                case CruiseControlInputs.TurnOnOrOff:
                    if (World.Instance.ControlledCar.cruiseControl.ACCenabled)
                    {
                        World.Instance.ControlledCar.cruiseControl.ACCenabled = !World.Instance.ControlledCar.cruiseControl.ACCenabled;
                    }
                    else if (!World.Instance.ControlledCar.cruiseControl.ACCenabled && World.Instance.ControlledCar.VirtualFunctionBus.PowertrainPacket.CurrentSpeed >= 30)
                    {
                        World.Instance.ControlledCar.cruiseControl.ACCenabled = !World.Instance.ControlledCar.cruiseControl.ACCenabled;
                    }
                    ControlMessenger.Instance.FireCruiseControlEvent(CruiseControlInputs.Empty);
                    break;
            }
        }
    }
}

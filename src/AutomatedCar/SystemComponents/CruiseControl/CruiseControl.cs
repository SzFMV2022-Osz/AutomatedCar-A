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

        private readonly double[] accDistances = new double[4] { 0.8, 1, 1.2, 1.4 };
        private int currentAccDistanceIdx;

        public CruiseControl(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.ACCenabled = false;
            this.currentAccDistanceIdx = 0;
        }

        /// <summary>
        /// Gets the current ACC distance value (e.g 0.8). OverflowException is being avoided using modulus.
        /// </summary>
        public double GetCurrentAccDistance
        {
            get
            {
                return this.accDistances[this.currentAccDistanceIdx % this.accDistances.Length];
            }
        }

        /// <summary>
        /// Sets the next possible ACC distance (e.g from 0.8 to 1).
        /// </summary>
        public void SetNextAccDistance()
        {
            this.currentAccDistanceIdx++;
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

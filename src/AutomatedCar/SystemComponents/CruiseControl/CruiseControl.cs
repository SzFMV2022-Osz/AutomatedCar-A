namespace AutomatedCar.SystemComponents.CruiseControl
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;

    /// <summary>
    /// This class represents the Cruise control feature.
    /// </summary>
    public class CruiseControl : SystemComponent
    {
        public bool ACCenabled;

        private readonly double[] accDistances = new double[4] { 0.8, 1, 1.2, 1.4 };
        private int currentAccDistanceIdx;

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControl"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">VBF parameter.</param>
        public CruiseControl(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
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

        /// <inheritdoc/>
        public override void Process()
        {
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
                case CruiseControlInputs.ChangeTargetDistance:
                    this.SetNextAccDistance();
                    break;
            }
        }
    }
}

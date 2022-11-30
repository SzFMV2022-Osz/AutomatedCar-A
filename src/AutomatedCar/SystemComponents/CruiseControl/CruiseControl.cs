namespace AutomatedCar.SystemComponents.CruiseControl
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using System.Diagnostics;

    /// <summary>
    /// This class represents the Cruise control feature.
    /// </summary>
    public class CruiseControl : SystemComponent
    {
        public bool ACCenabled;

        private readonly double[] accDistances = new double[4] { 0.8, 1, 1.2, 1.4 };
        private int currentAccDistanceIdx;
 
        private int targetSpeed;
        private int targetSpeedDiff = 10;
        private readonly int minTargetSpeed = 30;
        private readonly int maxTargetSpeed = 160;

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControl"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">VBF parameter.</param>
        public CruiseControl(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.ACCenabled = false;
            this.currentAccDistanceIdx = 0;
            this.targetSpeed = 0;
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
            CruiseControl selectedACC = World.Instance.ControlledCar.cruiseControl;

            // ONLY the controlled car should accept the inputs!
            if (selectedACC == this)
            {
                CruiseControlInputs input = CruiseControlInputs.Empty;

                // The inputs are placed into a ConcurrentQueue to avoid input spam.
                this.virtualFunctionBus.InputPacket.CruiseControlInputs.TryDequeue(out input);

                if (input != CruiseControlInputs.Empty)
                {
                    switch (input)
                    {
                        case CruiseControlInputs.TurnOff:
                            selectedACC.ACCenabled = false;
                            this.targetSpeed = 0;
                            break;
                        case CruiseControlInputs.TurnOnOrOff:
                            if (selectedACC.ACCenabled)
                            {
                                selectedACC.ACCenabled = !selectedACC.ACCenabled;
                                this.targetSpeed = 0;
                            }
                            else if (!selectedACC.ACCenabled)
                            {
                                selectedACC.ACCenabled = !selectedACC.ACCenabled;
                                if (World.Instance.ControlledCar.VirtualFunctionBus.PowertrainPacket.CurrentSpeed >= 160)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else
                                {
                                    this.targetSpeed = virtualFunctionBus.PowertrainPacket.CurrentSpeed;
                                }
                            }
                            Debug.WriteLine("ACCenabled: " + this.ACCenabled.ToString());
                            break;

                        case CruiseControlInputs.ChangeTargetDistance:
                            this.SetNextAccDistance();
                            Debug.WriteLine("Target distance: " + selectedACC.GetCurrentAccDistance);
                            break;

                        case CruiseControlInputs.IncreaseTargetSpeed:
                            if (this.ACCenabled)
                            {
                                if (virtualFunctionBus.PowertrainPacket.CurrentSpeed + this.targetSpeedDiff >= this.maxTargetSpeed)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else if (virtualFunctionBus.PowertrainPacket.CurrentSpeed + this.targetSpeedDiff <= this.minTargetSpeed)
                                {
                                    this.targetSpeed = this.minTargetSpeed;
                                }
                                else
                                {
                                    this.targetSpeed += this.targetSpeedDiff;
                                }
                            }
                            Debug.WriteLine("Target speed increased: " + this.targetSpeed);
                            break;

                        case CruiseControlInputs.DecreaseTargetSpeed:
                            if (this.ACCenabled)
                            {
                                if (virtualFunctionBus.PowertrainPacket.CurrentSpeed - this.targetSpeedDiff >= this.maxTargetSpeed)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else if (virtualFunctionBus.PowertrainPacket.CurrentSpeed - this.targetSpeedDiff <= this.minTargetSpeed)
                                {
                                    this.targetSpeed = this.minTargetSpeed;
                                }
                                else
                                {
                                    this.targetSpeed -= this.targetSpeedDiff;
                                }
                            }
                            Debug.WriteLine("Target speed decreased: " + this.targetSpeed);
                            break;
                    }
                }
            }
        }
    }
}

namespace AutomatedCar.SystemComponents.CruiseControl
{
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.Linq;

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
        private readonly AutomatedCar car;

        /// <summary>
        /// Initializes a new instance of the <see cref="CruiseControl"/> class.
        /// </summary>
        /// <param name="virtualFunctionBus">VBF parameter.</param>
        public CruiseControl(VirtualFunctionBus virtualFunctionBus, AutomatedCar car)
            : base(virtualFunctionBus)
        {
            this.ACCenabled = false;
            this.currentAccDistanceIdx = 0;
            this.targetSpeed = 0;
            this.car = car;
            this.virtualFunctionBus.CruiseControlPacket = new Packets.CruiseControlPacket();
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
            HandleInput();

            // Do the math
            if (this.ACCenabled)
            {
                double? distCarInFront = this.CalculateDistanceFromCarInFront();

                if (distCarInFront != null)
                {
                    this.HandleACC(distCarInFront ?? 0);
                }
                else
                {
                    this.HandleCC();
                }
            }

            // Save to packet
            this.virtualFunctionBus.CruiseControlPacket.ACCEnabled = this.ACCenabled;
            this.virtualFunctionBus.CruiseControlPacket.TargetDistance = this.GetCurrentAccDistance;
            this.virtualFunctionBus.CruiseControlPacket.TargetSpeed = this.targetSpeed;
        }

        private void HandleInput()
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
                                ControlMessenger.Instance.FirePedalEvent(Pedals.Empty);
                                this.targetSpeed = 0;
                            }
                            else if (!selectedACC.ACCenabled)
                            {
                                selectedACC.ACCenabled = !selectedACC.ACCenabled;
                                if (World.Instance.ControlledCar.VirtualFunctionBus.PowertrainPacket.CurrentSpeed >= this.maxTargetSpeed)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else if (World.Instance.ControlledCar.VirtualFunctionBus.PowertrainPacket.CurrentSpeed <= this.minTargetSpeed)
                                {
                                    this.targetSpeed = this.minTargetSpeed;
                                }
                                else
                                {
                                    this.targetSpeed = this.virtualFunctionBus.PowertrainPacket.CurrentSpeed;
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
                                if (this.targetSpeed + this.targetSpeedDiff >= this.maxTargetSpeed)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else if (this.targetSpeed + this.targetSpeedDiff <= this.minTargetSpeed)
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
                                if (this.targetSpeed - this.targetSpeedDiff >= this.maxTargetSpeed)
                                {
                                    this.targetSpeed = this.maxTargetSpeed;
                                }
                                else if (this.targetSpeed - this.targetSpeedDiff <= this.minTargetSpeed)
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

        private void HandleCC()
        {
            int currentSpeed = this.virtualFunctionBus.PowertrainPacket.CurrentSpeed;
            double carSpeedKph = currentSpeed; // this.PixelPerSecondToKph(currentSpeed);

            ControlMessenger.Instance.FirePedalEvent(Pedals.Empty);

            if (carSpeedKph < this.targetSpeed)
            {
                ControlMessenger.Instance.FirePedalEvent(Pedals.Gas);
            }
            else if (carSpeedKph > this.minTargetSpeed)
            {
                ControlMessenger.Instance.FirePedalEvent(Pedals.Brake);
            }
            else
            {
                this.ACCenabled = false; // TODO: megkérdezni mi történjen ha 30 kph alatt megy az előttünk lévő autó -> kikapcsolni az ACC-t és majd a vészfékező intézi?
            }
        }

        private void HandleACC(double distCarInFront)
        {
            int currentSpeed = this.virtualFunctionBus.PowertrainPacket.CurrentSpeed;
            double gap = (distCarInFront / 50) / (currentSpeed / 3.6);
            double gapGoal = this.accDistances[this.currentAccDistanceIdx];
            double carSpeedKph = currentSpeed; //this.PixelPerSecondToKph(currentSpeed);

            ControlMessenger.Instance.FirePedalEvent(Pedals.Empty);

            if (gap > gapGoal)
            {
                if (carSpeedKph < this.targetSpeed)
                {
                    ControlMessenger.Instance.FirePedalEvent(Pedals.Gas);
                }
                else
                {
                    ControlMessenger.Instance.FirePedalEvent(Pedals.Empty);
                }
            }
            else if (gap < gapGoal)
            {
                if (carSpeedKph > this.minTargetSpeed)
                {
                    ControlMessenger.Instance.FirePedalEvent(Pedals.Brake);
                }
                else
                {
                    ControlMessenger.Instance.FireCruiseControlEvent(CruiseControlInputs.TurnOnOrOff); // this.ACCenabled = false; // TODO: megkérdezni mi történjen ha 30 kph alatt megy az előttünk lévő autó -> kikapcsolni az ACC-t és majd a vészfékező intézi?
                }
            }
        }

        private double? CalculateDistanceFromCarInFront()
        {
            WorldObject carInFront = this.virtualFunctionBus.RadarPacket.RelevantWorldObjs.Where(x => x is Car || x is NpcCar).FirstOrDefault() as WorldObject;
            return carInFront != null ? this.GetDistance(carInFront.X, carInFront.Y, this.car.X, this.car.Y) : null;
        }

        private double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        private double PixelPerSecondToKph(double pixelPerSecond)
        {
            return pixelPerSecond / 50 * 3.6;
        }
    }
}

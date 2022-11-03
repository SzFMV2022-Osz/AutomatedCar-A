// <copyright file="PowertrainManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System.Diagnostics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Powertrain manager class is the head of powertrain system. It controls different systems within the engine.
    /// </summary>
    public class PowertrainManager : SystemComponent, IPowertrain
    {
        private IPowertrainPacket powertrainPacket;

        private IEngine engine;
        private ISteering steering;
        private IGearshift gearshift;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowertrainManager"/> class.
        /// Sets everything up for powertrain.
        /// </summary>
        /// <param name="virtualFunctionBus">An instance of the virtual function bus.</param>
        public PowertrainManager(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;
            this.gearshift = new Gearshift();
            this.engine = new Engine();
            this.steering = new Steering();
        }

        public void CalculateNextPosition()
        {
            double gasInputForce = this.engine.GetThrottleValue * 0.01;
            double brakeInputForce = this.engine.GetBrakeValue * 0.01;
            double slowingForce = this.engine.GetSpeed * 0.01 + (this.engine.GetSpeed > 0 ? brakeInputForce : 0);

            this.engine.Acceleration.Y = gasInputForce;

            this.engine.Velocity.Y = this.engine.GetVelocityAccordingToGear(slowingForce);

            this.engine.CalculateSpeed();
            this.steering.GetRotation();
            this.engine.CalculateRevolutions();
            if (this.gearshift.InnerShiftingStatus != Shifting.None)
            {
                this.engine.HandleRpmTransitionWhenShifting();
            }
        }

        /// <summary>
        /// Main process function controls the work of the powertrain.
        /// </summary>
        public override void Process()
        {
            // Debug.WriteLine(this.virtualFunctionBus.InputPacket.ToString());
            switch (this.virtualFunctionBus.InputPacket.PedalState)
            {
                case Pedals.Gas:
                    if (this.gearshift.State == GearshiftState.D || this.gearshift.State == GearshiftState.R)
                    {
                        this.engine.Accelerate();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        //World.Instance.ControlledCar.Y -= this.engine.GetSpeed;
                    }

                    this.virtualFunctionBus.PowertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                    this.virtualFunctionBus.PowertrainPacket.Rpm = this.engine.GetRPMValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    break;
                case Pedals.Brake:
                    if (this.gearshift.State == GearshiftState.D || this.gearshift.State == GearshiftState.R)
                    {
                        this.engine.Braking();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        //World.Instance.ControlledCar.Y -= this.engine.GetSpeed;
                    }

                    this.virtualFunctionBus.PowertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                    this.virtualFunctionBus.PowertrainPacket.Rpm = this.engine.GetRPMValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    break;
                case Pedals.Empty:
                    if (this.engine.GetSpeed > 0 || this.engine.GetSpeed < 0)
                    {
                        this.engine.Lift();
                        this.engine.LiftBraking();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        //World.Instance.ControlledCar.Y -= this.engine.GetSpeed;

                        this.virtualFunctionBus.PowertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                        this.virtualFunctionBus.PowertrainPacket.Rpm = this.engine.GetRPMValue;
                        this.virtualFunctionBus.PowertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                        this.virtualFunctionBus.PowertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    }

                    break;
            }

            if (this.virtualFunctionBus.InputPacket.IsGearStateJustChanged)
            {
                switch (this.virtualFunctionBus.InputPacket.GearState)
                {
                    case Gears.ShiftUp:
                        this.gearshift.StateUp((int)this.engine.Velocity.Y, (int)this.engine.GetSpeed);
                        if (this.gearshift.State == GearshiftState.P)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "P";
                        }
                        else if (this.gearshift.State == GearshiftState.R)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "R";
                        }
                        else if (this.gearshift.State == GearshiftState.N)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "N";
                        }
                        else
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "D";
                        }

                        break;
                    case Gears.ShiftDown:
                        this.gearshift.StateDown((int)this.engine.Velocity.Y, (int)this.engine.GetSpeed);
                        if (this.gearshift.State == GearshiftState.P)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "P";
                        }
                        else if (this.gearshift.State == GearshiftState.R)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "R";
                        }
                        else if (this.gearshift.State == GearshiftState.N)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "N";
                        }
                        else
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "D";
                        }

                        break;
                    case Gears.Steady:
                        break;
                }
            }

            this.steering.Seed(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y, World.Instance.ControlledCar.Rotation);
            switch (this.virtualFunctionBus.InputPacket.SteeringState)
            {
                case SteeringState.Left:
                    this.steering.TurnLeft();
                    CalculateNextPosition();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocationX;
                    World.Instance.ControlledCar.Y = (int)this.steering.CarLocationY;
                    //this.virtualFunctionBus.PowertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.virtualFunctionBus.PowertrainPacket.RotationAngle = this.steering.Rotation;
                    this.virtualFunctionBus.PowertrainPacket.Left = "L";
                    this.virtualFunctionBus.PowertrainPacket.Right = string.Empty;
                    break;
                case SteeringState.Right:
                    this.steering.TurnRight();
                    CalculateNextPosition();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocationX;
                    World.Instance.ControlledCar.Y = (int)this.steering.CarLocationY;
                    //this.virtualFunctionBus.PowertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.virtualFunctionBus.PowertrainPacket.RotationAngle = this.steering.Rotation;
                    this.virtualFunctionBus.PowertrainPacket.Left = string.Empty;
                    this.virtualFunctionBus.PowertrainPacket.Right = "R";
                    break;
                case SteeringState.Center:
                    this.steering.StraightenWheel();
                    CalculateNextPosition();
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocationX;
                    World.Instance.ControlledCar.Y = (int)this.steering.CarLocationY;
                    //this.virtualFunctionBus.PowertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.virtualFunctionBus.PowertrainPacket.RotationAngle = this.steering.Rotation;
                    this.virtualFunctionBus.PowertrainPacket.Left = string.Empty;
                    this.virtualFunctionBus.PowertrainPacket.Right = string.Empty;
                    break;
            }

            Debug.WriteLine(this.engine.GetSpeed);
            Debug.WriteLine(this.gearshift.State);
        }
    }
}

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
            this.engine = new Engine(this.gearshift);
            this.steering = new Steering();
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
                    if (this.engine.GetGearshiftState == GearshiftState.D || this.engine.GetGearshiftState == GearshiftState.R)
                    {
                        this.engine.Accelerate();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        World.Instance.ControlledCar.Y -= this.engine.GetSpeed;
                    }

                    this.powertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                    this.powertrainPacket.Rpm = this.engine.GetRPMValue;
                    this.powertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                    this.powertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    break;
                case Pedals.Brake:
                    if (this.engine.GetGearshiftState == GearshiftState.D || this.engine.GetGearshiftState == GearshiftState.R)
                    {
                        this.engine.Braking();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        World.Instance.ControlledCar.Y -= this.engine.GetSpeed;
                    }

                    this.powertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                    this.powertrainPacket.Rpm = this.engine.GetRPMValue;
                    this.powertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                    this.powertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    break;
                case Pedals.Empty:
                    if (this.engine.GetSpeed > 0 || this.engine.GetSpeed < 0)
                    {
                        this.engine.Lift();
                        this.steering.CarSpeed = this.engine.GetSpeed;
                        World.Instance.ControlledCar.Y -= this.engine.GetSpeed;

                        this.powertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                        this.powertrainPacket.Rpm = this.engine.GetRPMValue;
                        this.powertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                        this.powertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    }

                    break;
            }

            if (this.virtualFunctionBus.InputPacket.IsGearStateJustChanged)
            {
                switch (this.virtualFunctionBus.InputPacket.GearState)
                {
                    case Gears.ShiftUp:
                        this.engine.StateUp();
                        this.steering.State = this.engine.GetGearshiftState;
                        this.powertrainPacket.CurrentGear = this.engine.GetGearshiftState;
                        break;
                    case Gears.ShiftDown:
                        this.engine.StateDown();
                        this.steering.State = this.engine.GetGearshiftState;
                        this.powertrainPacket.CurrentGear = this.engine.GetGearshiftState;
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
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocation.X;
                    this.powertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.powertrainPacket.RotationAngle = this.steering.Rotation;
                    break;
                case SteeringState.Right:
                    this.steering.TurnRight();
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocation.X;
                    this.powertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.powertrainPacket.RotationAngle = this.steering.Rotation;
                    break;
                case SteeringState.Center:
                    this.steering.StraightenWheel();
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    World.Instance.ControlledCar.X = (int)this.steering.CarLocation.X;
                    this.powertrainPacket.Steering = this.virtualFunctionBus.InputPacket.SteeringState;
                    this.powertrainPacket.RotationAngle = this.steering.Rotation;
                    break;
            }

            Debug.WriteLine(this.engine.GetSpeed);
            Debug.WriteLine(this.engine.GetGearshiftState);
        }
    }
}

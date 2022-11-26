// <copyright file="PowertrainManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System.Diagnostics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using AutomatedCar.SystemComponents.Powertrain.Steering;

    /// <summary>
    /// Powertrain manager class is the head of powertrain system. It controls different systems within the engine.
    /// </summary>
    public class PowertrainManager : SystemComponent, IPowertrain
    {
        private IPowertrainPacket powertrainPacket;
        private IEngine engine;
        private SteeringV2 steeringV2;
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
            this.engine = new Engine(this.virtualFunctionBus, this.gearshift);
            this.steeringV2 = new SteeringV2();
        }

        /// <summary>
        /// Main process function controls the work of the powertrain.
        /// </summary>
        public override void Process()
        {
            switch (this.virtualFunctionBus.InputPacket.PedalState)
            {
                case Pedals.Gas:
                    if (this.engine.GetGearshiftState == GearshiftState.D || this.engine.GetGearshiftState == GearshiftState.R)
                    {
                        this.engine.Accelerate();
                    }

                    this.virtualFunctionBus.PowertrainPacket.CurrentSpeed = this.engine.GetSpeed;
                    this.virtualFunctionBus.PowertrainPacket.Rpm = this.engine.GetRPMValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentThrottleValue = this.engine.GetThrottleValue;
                    this.virtualFunctionBus.PowertrainPacket.CurrentBrakeValue = this.engine.GetBrakeValue;
                    break;
                case Pedals.Brake:
                    if (this.engine.GetGearshiftState == GearshiftState.D || this.engine.GetGearshiftState == GearshiftState.R)
                    {
                        this.engine.Braking();
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
                        this.engine.StateUp();
                        if (this.engine.GetGearshiftState == GearshiftState.P)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "P";
                        }
                        else if (this.engine.GetGearshiftState == GearshiftState.R)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "R";
                        }
                        else if (this.engine.GetGearshiftState == GearshiftState.N)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "N";
                        }
                        else
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "D";
                        }

                        break;
                    case Gears.ShiftDown:
                        this.engine.StateDown();
                        if (this.engine.GetGearshiftState == GearshiftState.P)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "P";
                        }
                        else if (this.engine.GetGearshiftState == GearshiftState.R)
                        {
                            this.virtualFunctionBus.PowertrainPacket.CurrentGear = "R";
                        }
                        else if (this.engine.GetGearshiftState == GearshiftState.N)
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

            switch (this.virtualFunctionBus.InputPacket.SteeringState)
            {
                case SteeringState.Left:
                    this.steeringV2.SteeringLeft();
                    this.virtualFunctionBus.PowertrainPacket.Left = "L";
                    this.virtualFunctionBus.PowertrainPacket.Right = string.Empty;
                    break;
                case SteeringState.Right:
                    this.steeringV2.SteeringRight();
                    this.virtualFunctionBus.PowertrainPacket.Left = string.Empty;
                    this.virtualFunctionBus.PowertrainPacket.Right = "R";
                    break;
                case SteeringState.Center:
                    this.steeringV2.SteeringCenter();
                    this.virtualFunctionBus.PowertrainPacket.Left = string.Empty;
                    this.virtualFunctionBus.PowertrainPacket.Right = string.Empty;
                    break;
            }

            this.virtualFunctionBus.PowertrainPacket.RotationAngle = this.steeringV2.SteeringAngle;
            this.steeringV2.SteeringAndSetNewCarPosition(this.engine.GetSpeed, World.Instance.ControlledCar);
        }
    }
}

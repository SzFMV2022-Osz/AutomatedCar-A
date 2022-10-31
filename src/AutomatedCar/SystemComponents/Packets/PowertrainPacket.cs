// <copyright file="PowertrainPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Powertrain;
    using ReactiveUI;

    /// <summary>
    /// Packet for the VFB to communicate with other systems.
    /// </summary>
    public class PowertrainPacket : ReactiveObject, IPowertrainPacket
    {
        /// <summary>
        /// Gets or sets the percentage of the gas pedal.
        /// </summary>
        public int CurrentThrottleValue { get; set; }

        /// <summary>
        /// Gets or sets the percentage of the brake pedal.
        /// </summary>
        public int CurrentBrakeValue { get; set; }

        /// <summary>
        /// Gets or sets the current speed of the car in Km/h.
        /// </summary>
        public int CurrentSpeed { get; set; }

        /// <summary>
        /// Gets or sets the current gearbox state. Values: P,R,N,D.
        /// </summary>
        public GearshiftState CurrentGear { get; set; }

        /// <summary>
        /// Gets or sets the current revolution of the engine.
        /// </summary>
        public int Rpm { get; set; }

        /// <summary>
        /// Gets or sets the current steering state. Values: Left, Right, Center.
        /// </summary>
        public SteeringState Steering { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the steering wheel.
        /// </summary>
        public double RotationAngle { get; set; }
    }
}

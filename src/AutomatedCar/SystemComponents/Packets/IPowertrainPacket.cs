// <copyright file="IPowertrainPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Powertrain;

    /// <summary>
    /// Interface of a packet for the VFB to communicate with other systems.
    /// </summary>
    public interface IPowertrainPacket
    {
        /// <summary>
        /// Gets or sets the percentage of the gas pedal.
        /// </summary>
        int CurrentThrottleValue { get; set; }

        /// <summary>
        /// Gets or sets the percentage of the brake pedal.
        /// </summary>
        int CurrentBrakeValue { get; set; }

        /// <summary>
        /// Gets or sets the current speed of the car in Km/h.
        /// </summary>
        int CurrentSpeed { get; set; }

        /// <summary>
        /// Gets or sets the current gearbox state. Values: P,R,N,D.
        /// </summary>
        GearshiftState CurrentGear { get; set; }

        /// <summary>
        /// Gets or sets the current revolution of the engine.
        /// </summary>
        int Rpm { get; set; }

        /// <summary>
        /// Gets or sets the current steering state. Values: Left, Right, Center.
        /// </summary>
        SteeringState Steering { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the steering wheel.
        /// </summary>
        double RotationAngle { get; set; }
    }
}
// <copyright file="IControlMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.Messenger
{
    using System;

    /// <summary>
    /// Interface for Control Messenger to communicate with the powertrain.
    /// </summary>
    public interface IControlMessenger
    {
        /// <summary>
        /// Event for the steering.
        /// </summary>
        static event EventHandler<ControlEventArgs> SteeringEventHandler;

        /// <summary>
        /// Event for the pedals.
        /// </summary>
        static event EventHandler<ControlEventArgs> PedalsEventHandler;

        /// <summary>
        /// Event for the gears.
        /// </summary>
        static event EventHandler<ControlEventArgs> GearsEventHandler;

        /// <summary>
        /// Gets or sets the position of steering wheel.
        /// </summary>
        Steering Steering { get; set; }

        /// <summary>
        /// Gets or sets the position of pedals.
        /// </summary>
        Pedals Pedal { get; set; }

        /// <summary>
        /// Gets or sets the position of the transmission gearshift.
        /// </summary>
        Gears Gear { get; set; }
    }
}

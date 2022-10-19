// <copyright file="IControlMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.InputManager.Messenger
{
    using System;

    /// <summary>
    /// Interface for Control Messenger to communicate with the powertrain.
    /// </summary>
    public interface IControlMessenger
    {
        /// <summary>
        /// Event for the powertrain.
        /// </summary>
        static event EventHandler ControleEventHandler;

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

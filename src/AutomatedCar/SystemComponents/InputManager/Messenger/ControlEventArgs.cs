// <copyright file="ControlEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.InputManager.Messenger
{
    using System;

    /// <summary>
    /// Eventrgs for communication.
    /// </summary>
    public class ControlEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the position of steering wheel.
        /// </summary>
        public Steering Steering { get; set; }

        /// <summary>
        /// Gets or sets the position of pedals.
        /// </summary>
        public Pedals Pedal { get; set; }

        /// <summary>
        /// Gets or sets the position of the transmission gearshift.
        /// </summary>
        public Gears Gear { get; set; }
    }
}

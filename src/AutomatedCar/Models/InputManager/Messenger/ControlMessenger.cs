// <copyright file="ControlMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.InputManager.Messenger
{
    using System;

    /// <summary>
    /// Control Messenger class for the communication.
    /// </summary>
    public class ControlMessenger : IControlMessenger
    {
        private Steering steering;

        private Pedals pedal;

        private Gears gears;

        /// <summary>
        /// Event for the steering.
        /// </summary>
        public static event EventHandler SteeringEventHandler;

        /// <summary>
        /// Event for the pedals.
        /// </summary>
        public static event EventHandler PedalEventHandler;

        /// <summary>
        /// Event for the gearbox.
        /// </summary>
        public static event EventHandler GearboxEventHandler;

        /// <summary>
        /// Gets or sets the position of steering wheel.
        /// </summary>
        public Steering Steering
        {
            get
            {
                return this.steering;
            }

            set
            {
                EventHandler handler = ControlMessenger.SteeringEventHandler;
                ControlEventArgs eventArgs = new ControlEventArgs();
                if (handler != null)
                {
                    eventArgs.Steering = value;
                    this.steering = value;
                    handler?.Invoke(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of pedals.
        /// </summary>
        public Pedals Pedal
        {
            get
            {
                return this.pedal;
            }

            set
            {
                EventHandler handler = ControlMessenger.PedalEventHandler;
                ControlEventArgs eventArgs = new ControlEventArgs();
                if (handler != null)
                {
                    eventArgs.Pedal = value;
                    this.pedal = value;
                    handler?.Invoke(this, eventArgs);
                }
            }
        }

        /// <summary>
        /// Gets or sets the position of the transmission gearshift.
        /// </summary>
        public Gears Gear
        {
            get
            {
                return this.gears;
            }

            set
            {
                EventHandler handler = ControlMessenger.GearboxEventHandler;
                ControlEventArgs eventArgs = new ControlEventArgs();
                if (handler != null)
                {
                    eventArgs.Gear = value;
                    this.gears = value;
                    handler?.Invoke(this, eventArgs);
                }
            }
        }
    }
}

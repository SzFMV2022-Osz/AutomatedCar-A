// <copyright file="ControlMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.Messenger
{
    using System;

    /// <summary>
    /// Control Messenger class for the communication.
    /// </summary>
    public class ControlMessenger : IControlMessenger
    {
        private SteeringState steering;

        private Pedals pedal;

        private Gears gears;

        /// <summary>
        /// Event for the steering.
        /// </summary>
        public static event EventHandler<ControlEventArgs> SteeringEventHandler;

        /// <summary>
        /// Event for the pedals.
        /// </summary>
        public static event EventHandler<ControlEventArgs> PedalEventHandler;

        /// <summary>
        /// Event for the gearbox.
        /// </summary>
        public static event EventHandler<ControlEventArgs> GearboxEventHandler;

        /// <summary>
        /// Gets or sets the position of steering wheel.
        /// </summary>
        public SteeringState Steering
        {
            get
            {
                return this.steering;
            }

            set
            {
                var handler = SteeringEventHandler;
                var eventArgs = new ControlEventArgs();

                if (handler == null)
                {
                    return;
                }

                eventArgs.Steering = value;
                this.steering = value;
                handler?.Invoke(this, eventArgs);
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
                var handler = PedalEventHandler;
                var eventArgs = new ControlEventArgs();

                if (handler == null)
                {
                    return;
                }

                eventArgs.Pedal = value;
                this.pedal = value;
                handler?.Invoke(this, eventArgs);
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
                var handler = GearboxEventHandler;
                var eventArgs = new ControlEventArgs();

                if (handler == null)
                {
                    return;
                }

                eventArgs.Gear = value;
                this.gears = value;
                handler?.Invoke(this, eventArgs);
            }
        }
    }
}

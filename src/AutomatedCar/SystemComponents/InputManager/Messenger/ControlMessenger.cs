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
        private static readonly object cm_lock = new object();

        private static ControlMessenger instance = null;

        public static ControlMessenger Instance
        {
            get
            {
                lock (cm_lock)
                {
                    if (instance == null)
                    {
                        instance = new ControlMessenger();
                    }

                    return instance;
                }
            }
        }

        private SteeringState steering;

        private Pedals pedal;

        private Gears gears;

        /// <summary>
        /// Event for the steering.
        /// </summary>
        public event EventHandler<ControlEventArgs> SteeringEventHandler;

        /// <summary>
        /// Event for the pedals.
        /// </summary>
        public event EventHandler<ControlEventArgs> PedalEventHandler;

        /// <summary>
        /// Event for the gearbox.
        /// </summary>
        public event EventHandler<ControlEventArgs> GearboxEventHandler;

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
                EventHandler<ControlEventArgs> handler = SteeringEventHandler;
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
                EventHandler<ControlEventArgs> handler = PedalEventHandler;
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
                EventHandler<ControlEventArgs> handler = GearboxEventHandler;
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

// <copyright file="ControlMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.Messenger
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Control Messenger class for the communication.
    /// </summary>
    public class ControlMessenger : IControlMessenger
    {
        private static readonly object CMLOCK = new object();

        private static ControlMessenger instance = null;

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
        /// Event for the cruise control.
        /// </summary>
        public event EventHandler<ControlEventArgs> CruiseControlEventHandler;

        /// <summary>
        /// Gets a ControlMessenger instance.
        /// </summary>
        public static ControlMessenger Instance
        {
            get
            {
                lock (CMLOCK)
                {
                    if (instance == null)
                    {
                        instance = new ControlMessenger();
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="steeringState">Gets a steeringtate for the steering wheel.</param>
        public void FireSteeringEvent(SteeringState steeringState)
        {
            ControlEventArgs eventArgs = new ControlEventArgs();
            if (this.SteeringEventHandler != null)
            {
                eventArgs.Steering = steeringState;
                this.SteeringEventHandler?.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="pedalState">Gets a steeringtate for the steering wheel.</param>
        public void FirePedalEvent(Pedals pedalState)
        {
            ControlEventArgs eventArgs = new ControlEventArgs();
            if (this.PedalEventHandler != null)
            {
                eventArgs.Pedal = pedalState;
                this.PedalEventHandler?.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="gearState">Gets a steeringtate for the steering wheel.</param>
        public void FireGearboxEvent(Gears gearState)
        {
            ControlEventArgs eventArgs = new ControlEventArgs();
            if (this.GearboxEventHandler != null)
            {
                eventArgs.Gear = gearState;
                this.GearboxEventHandler?.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// Fires the event for cruise control input received.
        /// </summary>
        /// <param name="cruiseControlInput">The type of input received for the cruise control.</param>
        public void FireCruiseControlEvent(CruiseControlInputs cruiseControlInput)
        {
            Debug.WriteLine(cruiseControlInput.ToString());
            ControlEventArgs eventArgs = new ControlEventArgs();
            if (this.CruiseControlEventHandler != null)
            {
                eventArgs.CruiseControlInput = cruiseControlInput;
                this.CruiseControlEventHandler?.Invoke(this, eventArgs);
            }
        }
    }
}

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
        event EventHandler<ControlEventArgs> SteeringEventHandler;

        /// <summary>
        /// Event for the pedals.
        /// </summary>
        event EventHandler<ControlEventArgs> PedalEventHandler;

        /// <summary>
        /// Event for the gears.
        /// </summary>
        event EventHandler<ControlEventArgs> GearboxEventHandler;

        /// <summary>
        /// Event for the cruise control.
        /// </summary>
        event EventHandler<ControlEventArgs> CruiseControlEventHandler;

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="steeringState">Gets a steeringtate for the steering wheel.</param>
        public void FireSteeringEvent(SteeringState steeringState);

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="pedalState">Gets a steeringtate for the steering wheel.</param>
        public void FirePedalEvent(Pedals pedalState);

        /// <summary>
        /// Fires the event for steering wheel state change.
        /// </summary>
        /// <param name="gearState">Gets a steeringtate for the steering wheel.</param>
        public void FireGearboxEvent(Gears gearState);

        /// <summary>
        /// Fires the even for cruise control controlling.
        /// </summary>
        /// <param name="cruiseControlInput">Tells the cruise control what type of input was received.</param>
        public void FireCruiseControlEvent(CruiseControlInputs cruiseControlInput);
    }
}

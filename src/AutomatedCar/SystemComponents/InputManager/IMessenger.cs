// <copyright file="IMessenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager
{
    using System;
    using AutomatedCar.Models.PowerTrain;

    /// <summary>
    /// Interface of message.
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// On pedal input changed.
        /// </summary>
        event EventHandler<PedalStates> OnPedalChanged;

        /// <summary>
        /// On steering input changed.
        /// </summary>
        event EventHandler<SteeringState> OnSteeringChanged;

        /// <summary>
        /// On shift input changed.
        /// </summary>
        event EventHandler<ShiftStates> OnShiftStateChanged;

        /// <summary>
        /// Send message to powertrain when pedal state changed.
        /// </summary>
        /// <param name="pedalStates">Pedal state.</param>
        void SendMessageToPowertrain(PedalStates pedalStates);

        /// <summary>
        /// Send message to powertrain when steering state changed.
        /// </summary>
        /// <param name="steeringStates">Steering state.</param>
        void SendMessageToPowertrain(SteeringState steeringStates);

        /// <summary>
        /// Send message to powertrain when shift state changed.
        /// </summary>
        /// <param name="shiftStates">shift state.</param>
        void SendMessageToPowertrain(ShiftStates shiftStates);
    }
}
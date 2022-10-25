// <copyright file="Messenger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager
{
    using System;
    using AutomatedCar.Models.PowerTrain;

    /// <summary>
    /// Mssenger.
    /// </summary>
    internal class Messenger : IMessenger
    {
        /// <summary>
        /// On pedal input changed.
        /// </summary>
        public event EventHandler<PedalStates> OnPedalChanged;

        /// <summary>
        /// On steering input changed.
        /// </summary>
        public event EventHandler<SteeringState> OnSteeringChanged;

        /// <summary>
        /// On shift input changed.
        /// </summary>
        public event EventHandler<ShiftStates> OnShiftStateChanged;

        /// <summary>
        /// Send message to powertrain when pedal state changed.
        /// </summary>
        /// <param name="pedalStates">Pedal state.</param>
        public void SendMessageToPowertrain(PedalStates pedalStates)
        {
            this.OnPedalChanged?.Invoke(this, pedalStates);
        }

        /// <summary>
        /// Send message to powertrain when steering state changed.
        /// </summary>
        /// <param name="steeringStates">Steering state.</param>
        public void SendMessageToPowertrain(SteeringState steeringStates)
        {
            this.OnSteeringChanged?.Invoke(this, steeringStates);
        }

        /// <summary>
        /// Send message to powertrain when shift state changed.
        /// </summary>
        /// <param name="shiftStates">shift state.</param>
        public void SendMessageToPowertrain(ShiftStates shiftStates)
        {
            this.OnShiftStateChanged?.Invoke(this, shiftStates);
        }
    }
}

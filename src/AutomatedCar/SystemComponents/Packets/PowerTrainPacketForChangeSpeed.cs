// <copyright file="PowerTrainPacketForChangeSpeed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain.Packets;
    using ReactiveUI;

    /// <summary>
    /// Send input for powertain.
    /// </summary>
    public class PowerTrainPacketForChangeSpeed : ReactiveObject, IPowerTrainPacketForChangeSpeed
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerTrainPacketForChangeSpeed"/> class.
        /// </summary>
        /// <param name="state">State.</param>
        public PowerTrainPacketForChangeSpeed(SpeedStates state)
        {
            this.State = state;
        }

        /// <summary>
        /// Gets State.
        /// </summary>
        public SpeedStates State { get; private set; }
    }
}

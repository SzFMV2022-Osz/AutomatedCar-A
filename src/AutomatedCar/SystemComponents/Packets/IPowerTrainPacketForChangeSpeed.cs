// <copyright file="IPowerTrainPacketForChangeSpeed.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain.Packets;

    /// <summary>
    /// Send speed input for powertain.
    /// </summary>
    public interface IPowerTrainPacketForChangeSpeed
    {
        /// <summary>
        /// Gets State.
        /// </summary>
        public SpeedStates State { get;  }
    }
}

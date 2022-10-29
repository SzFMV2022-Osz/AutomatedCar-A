// <copyright file="IPowerTrainPacketForChangeSteering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain;

    /// <summary>
    /// Send steering input for powertain.
    /// </summary>
    public interface IPowerTrainPacketForChangeSteering
    {
        /// <summary>
        /// Gets steering command.
        /// </summary>
        public SteeringState Command { get; }
    }
}

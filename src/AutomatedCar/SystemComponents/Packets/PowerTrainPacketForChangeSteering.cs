// <copyright file="PowerTrainPacketForChangeSteering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain;
    using ReactiveUI;

    /// <summary>
    /// Send steering input for powertain.
    /// </summary>
    internal class PowerTrainPacketForChangeSteering : ReactiveObject, IPowerTrainPacketForChangeSteering
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerTrainPacketForChangeSteering"/> class.
        /// </summary>
        /// <param name="command">Steering command.</param>
        public PowerTrainPacketForChangeSteering(SteeringState command)
        {
            this.Command = command;
        }

        /// <summary>
        /// Gets steering command.
        /// </summary>
        public SteeringState Command { get; private set; }
    }
}

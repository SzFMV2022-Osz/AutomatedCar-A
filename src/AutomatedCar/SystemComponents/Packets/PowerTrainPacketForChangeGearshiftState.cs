// <copyright file="PowerTrainPacketForChangeGearshiftState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain;
    using AutomatedCar.SystemComponents.Powertrain;
    using ReactiveUI;

    /// <summary>
    /// Send gearshift input for powertain.
    /// </summary>
    public class PowerTrainPacketForChangeGearshiftState : ReactiveObject, IPowerTrainPacketForChangeGearshiftState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerTrainPacketForChangeGearshiftState"/> class.
        /// </summary>
        /// <param name="command">Command for the gearsift.</param>
        public PowerTrainPacketForChangeGearshiftState(ShiftStates command)
        {
            this.GearshiftCommand = command;
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ShiftStates GearshiftCommand { get; private set; }
    }
}

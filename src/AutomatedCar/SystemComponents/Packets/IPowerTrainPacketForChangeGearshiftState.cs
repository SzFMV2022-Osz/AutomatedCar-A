// <copyright file="IPowerTrainPacketForChangeGearshiftState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models.PowerTrain;
    using AutomatedCar.SystemComponents.Powertrain;

    /// <summary>
    /// Send gearshift input for powertain.
    /// </summary>
    public interface IPowerTrainPacketForChangeGearshiftState
    {
        /// <summary>
        /// Gets the command for gearshift.
        /// </summary>
        public ShiftStates GearshiftCommand { get; }
    }
}

// <copyright file="SpeedStates.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.PowerTrain.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Speed states for powertrain packets.
    /// </summary>
    public enum SpeedStates
    {
        /// <summary>
        /// Speed up the car.
        /// </summary>
        SpeedUp,

        /// <summary>
        /// Slow down the car.
        /// </summary>
        Break,

        /// <summary>
        /// No command for powertrain.
        /// </summary>
        None,
    }
}

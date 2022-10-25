// <copyright file="SteeringState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.PowerTrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Steering states.
    /// </summary>
    public enum SteeringState
    {
        /// <summary>
        /// No stering input.
        /// </summary>
        Center,

        /// <summary>
        /// Turn left steering input.
        /// </summary>
        Left,

        /// <summary>
        /// turn right steering input.
        /// </summary>
        Right,
    }
}

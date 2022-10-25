// <copyright file="ShiftState.cs" company="PlaceholderCompany">
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
    /// Pedal states.
    /// </summary>
    public enum ShiftStates
    {
        /// <summary>
        /// Shift to next state (p,n,r,d).
        /// </summary>
        ShiftStateNext,

        /// <summary>
        /// Shift to prewious state (p,n,r,d).
        /// </summary>
        ShiftStatePrew,
    }
}

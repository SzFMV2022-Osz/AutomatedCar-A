// <copyright file="IGearshift.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// GearShift interface.
    /// </summary>
    public interface IGearshift
    {
        /// <summary>
        /// Gets or sets state.
        /// </summary>
        /// <returns>state.</returns>
        GearshiftState State { get; set; }

        /// <summary>
        /// Gets or sets state.
        /// </summary>
        /// <returns>state.</returns>
        Shifting InnerShiftingStatus { get; set; }

        /// <summary>
        /// Shifts up.
        /// </summary>
        void ShiftUp();

        /// <summary>
        /// Shifts down.
        /// </summary>
        void ShiftDown();

        /// <summary>
        /// Switch state dawn.
        /// </summary>
        void StateDown(int velocity, int speed);

        /// <summary>
        /// Switch state up.
        /// </summary>
        void StateUp(int velocity, int speed);
    }
}

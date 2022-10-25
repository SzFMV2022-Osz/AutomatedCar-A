// <copyright file="PedalStates.cs" company="PlaceholderCompany">
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
    public enum PedalStates
    {
        /// <summary>
        /// No pedals pressed.
        /// </summary>
        None,

        /// <summary>
        /// Break pedals pressed.
        /// </summary>
        Break,

        /// <summary>
        /// Throtle pedals pressed.
        /// </summary>
        Throtle,
    }
}

// <copyright file="IEngine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engine interface.
    /// </summary>
    internal interface IEngine
    {
        /// <summary>
        /// Gets speed of the car (km/h).
        /// </summary>
        int Speed { get; }

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        float Accelerate();

        /// <summary>
        /// Slows the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        float Lift();

        /// <summary>
        /// Breaks the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        float Breaking();

        /// <summary>
        /// Switch state dawn.
        /// </summary>
        void StateDown();

        /// <summary>
        /// Switch state up.
        /// </summary>
        void StateUp();
    }
}

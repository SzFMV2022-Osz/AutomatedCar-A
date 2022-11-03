﻿// <copyright file="IEngine.cs" company="PlaceholderCompany">
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
    /// Engine interface.
    /// </summary>
    internal interface IEngine
    {
        /// <summary>
        /// Gets speed of the car (km/h).
        /// </summary>
        int Speed { get; }

        /// <summary>
        /// Gets speed(+/-) of the car.
        /// </summary>
        public int SignSpeed { get; }

        /// <summary>
        /// Gets Rpm.
        /// </summary>
        float RPM { get; }

        /// <summary>
        /// Gets gearshift state.
        /// </summary>
        string GearShiftState { get; }

        /// <summary>
        /// Gets throtle pedal %.
        /// </summary>
        float Throtle { get; }

        /// <summary>
        /// Gets break pedal %.
        /// </summary>
        float Break { get; }

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

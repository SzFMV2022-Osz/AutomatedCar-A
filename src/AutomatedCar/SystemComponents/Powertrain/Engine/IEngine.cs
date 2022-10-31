// <copyright file="IEngine.cs" company="PlaceholderCompany">
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
    public interface IEngine
    {
        /// <summary>
        /// Gets speed of the car (km/h).
        /// </summary>
        int GetSpeed { get; }

        /// <summary>
        /// Gets the state of the gearbox. Only for debug purposes.
        /// </summary>
        GearshiftState GetGearshiftState { get; }

        /// <summary>
        /// Gets RPM of the car.
        /// </summary>
        int GetRPMValue { get; }

        /// <summary>
        /// Gets the percentage value of the throttle.
        /// </summary>
        int GetThrottleValue { get; }

        /// <summary>
        /// Gets the percentage value of the brake.
        /// </summary>
        int GetBrakeValue { get; }

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
        float Braking();

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

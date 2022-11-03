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

        Vector Acceleration { get; set; }

        Vector Velocity { get; set; }

        /// <summary>
        /// Gets the state of the gearbox. Only for debug purposes.
        /// </summary>
        //GearshiftState GetGearshiftState { get; }

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

        void CalculateSpeed();

        double GetVelocityAccordingToGear(double slowingForce);

        void CalculateRevolutions();

        void HandleRpmTransitionWhenShifting();

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        void Accelerate();

        /// <summary>
        /// Slows the car.
        /// </summary>
        void Lift();

        /// <summary>
        /// Breaks the car.
        /// </summary>
        void Braking();

        void LiftBraking();
    }
}

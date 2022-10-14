// <copyright file="ISteering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    /// <summary>
    /// Steering interface.
    /// </summary>
    internal interface ISteering
    {
        /// <summary>
        /// Finds the location of the wheels.
        /// </summary>
        void FindNewWheelLocations();

        /// <summary>
        /// Calculates the new wheel locations.
        /// </summary>
        void FindWheelLocations();

        /// <summary>
        /// Calculates the new heading of the car.
        /// </summary>
        void GetNewHeading();
    }
}
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
        /// Sets the turning direction to the left side.
        /// </summary>
        void TurnLeft();

        /// <summary>
        /// Sets the turning direction to the right side.
        /// </summary>
        void TurnRight();

        /// <summary>
        /// Resets the wheel to a straight position.
        /// </summary>
        void StraightenWheel();

        /// <summary>
        /// Calculates the rotation of the car.
        /// </summary>
        void GetRotation();
    }
}
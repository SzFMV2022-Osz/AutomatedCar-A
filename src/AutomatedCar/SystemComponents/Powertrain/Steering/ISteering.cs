﻿// <copyright file="ISteering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    /// <summary>
    /// Steering interface.
    /// </summary>
    public interface ISteering
    {
        /// <summary>
        /// Gets or sets the state of the Gearshift.
        /// </summary>
        int CarSpeed { get; set; }

        /// <summary>
        /// Gets or sets the state of the Gearshift.
        /// </summary>
        double Rotation { get; set; }

        /// <summary>
        /// Gets or sets the state of the Gearshift.
        /// </summary>
        GearshiftState State { get; set; }

        /// <summary>
        /// Sets up some info coming from the controlled car.
        /// </summary>
        /// <param name="x">X position of car.</param>
        /// <param name="y">Y position of car.</param>
        /// <param name="rotation">Rotation of the car.</param>
        void Seed(int x, int y, double rotation);

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
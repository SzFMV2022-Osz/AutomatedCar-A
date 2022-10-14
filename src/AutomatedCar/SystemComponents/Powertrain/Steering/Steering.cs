// <copyright file="Steering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using Vector = Avalonia.Vector;

    /// <summary>
    /// Steering.
    /// </summary>
    internal class Steering : ISteering
    {
        // atan(wheelBase / (turningCircle - carWidth)) = turningAngle
        private float wheelBase;
        private double steerAngle;

        private Vector carLocation;
        private double carHeading;
        private float carSpeed;

        private Vector frontWheel;
        private Vector backWheel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Steering"/> class.
        /// </summary>
        /// <param name="steerAngle">Angle of steering.</param>
        /// <param name="x">X position of car.</param>
        /// <param name="y">Y position of car.</param>
        /// <param name="carSpeed">Speed of car.</param>
        public Steering(double steerAngle, int x, int y, float carSpeed)
        {
            this.wheelBase = 130f;
            this.steerAngle = steerAngle;
            this.carLocation = new Vector(x, y);
            this.carSpeed = carSpeed;
        }

        /// <summary>
        /// Finds the location of the wheels.
        /// </summary>
        public void FindWheelLocations()
        {
            this.frontWheel = this.carLocation + (this.wheelBase / 2 * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading)));
            this.backWheel = this.carLocation - (this.wheelBase / 2 * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading)));
        }

        /// <summary>
        /// Calculates the new wheel locations.
        /// </summary>
        public void FindNewWheelLocations()
        {
            this.backWheel += this.carSpeed * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading));
            this.frontWheel += this.carSpeed * new Vector(Math.Cos(this.carHeading + this.steerAngle), Math.Sin(this.carHeading + this.steerAngle));
        }

        /// <summary>
        /// Calculates the new heading of the car.
        /// </summary>
        public void GetNewHeading()
        {
            this.carLocation = (this.frontWheel + this.backWheel) / 2;
            this.carHeading = Math.Atan2(this.frontWheel.Y - this.backWheel.Y, this.frontWheel.X - this.backWheel.X);
        }
    }
}

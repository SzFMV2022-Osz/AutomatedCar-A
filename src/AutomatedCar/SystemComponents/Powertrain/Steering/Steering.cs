// <copyright file="Steering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Vector = Avalonia.Vector;

    /// <summary>
    /// Steering.
    /// </summary>
    public class Steering : ISteering
    {
        private const double TurningOffset = 5;

        private static readonly Dictionary<int, double> ScalingValueLookupTable = new Dictionary<int, double>()
        {
                { 20, 1.0 },
                { 30, 0.9 },
                { 40, 0.8 },
                { 50, 0.7 },
                { 60, 0.6 },
                { 75, 0.5 },
                { 100, 0.4 },
        };

        private int wheelBase;
        private double steerAngle;

        private Vector carLocation;
        private double carHeading;
        private int carSpeed;

        private double rotation;

        private Vector frontWheel;
        private Vector backWheel;

        /// <summary>
        /// Initializes a new instance of the <see cref="Steering"/> class.
        /// </summary>
        public Steering()
        {
            this.wheelBase = 250;
            this.steerAngle = 0;
            this.carHeading = -1.5;
        }

        /// <summary>
        /// Gets or sets the location of the Car.
        /// </summary>
        public Vector CarLocation
        {
            get { return this.carLocation; }
            set { this.carLocation = value; }
        }

        /// <summary>
        /// Gets or sets the speed of the Car.
        /// </summary>
        public int CarSpeed
        {
            get { return this.carSpeed; }
            set { this.carSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the Car.
        /// </summary>
        public double Rotation
        {
            get { return this.rotation; } set { this.rotation = value; }
        }

        /// <summary>
        /// Sets up some info coming from the controlled car.
        /// </summary>
        /// <param name="x">X position of car.</param>
        /// <param name="y">Y position of car.</param>
        /// <param name="rotation">Rotation of the car.</param>
        public void Seed(int x, int y, double rotation)
        {
            this.carLocation = new Vector(x, y);
            this.rotation = rotation;
        }

        /// <summary>
        /// Sets the turning direction to the left side.
        /// </summary>
        public void TurnLeft()
        {
            this.steerAngle = TurningOffset;
        }

        /// <summary>
        /// Turning right.
        /// </summary>
        public void TurnRight()
        {
            this.steerAngle = -TurningOffset;
        }

        /// <summary>
        /// Resets the wheel to a straight position.
        /// </summary>
        public void StraightenWheel()
        {
            if (this.steerAngle < 0)
            {
                this.steerAngle += TurningOffset;
            }
            else if (this.steerAngle > 0)
            {
                this.steerAngle -= TurningOffset;
            }
        }

        /// <summary>
        /// Calculates the rotation of the car.
        /// </summary>
        public void GetRotation()
        {
            this.FindWheelLocations();
            this.FindNewWheelLocations();
            this.GetNewHeading();

            this.rotation = ((this.carHeading * 180) / Math.PI) + 87;
        }

        private void FindWheelLocations()
        {
            this.frontWheel = this.carLocation + (this.wheelBase / 2 * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading)));
            this.backWheel = this.carLocation - (this.wheelBase / 2 * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading)));
        }

        private void FindNewWheelLocations()
        {
            double scaling = this.GetScaleDownValue(this.carSpeed);
            this.backWheel += this.carSpeed * scaling * 3 * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading));
            this.frontWheel += this.carSpeed * scaling * 3 * new Vector(Math.Cos(this.carHeading + this.steerAngle), Math.Sin(this.carHeading + this.steerAngle));
        }

        private void GetNewHeading()
        {
            this.carLocation = (this.frontWheel + this.backWheel) / 2;
            this.carHeading = Math.Atan2(this.frontWheel.Y - this.backWheel.Y, this.frontWheel.X - this.backWheel.X);
        }

        private double GetScaleDownValue(int speed)
        {
            int rounded_speed = ScalingValueLookupTable.Keys.ToList().OrderBy(x => Math.Abs(speed - x)).First();
            return ScalingValueLookupTable[rounded_speed];
        }
    }
}

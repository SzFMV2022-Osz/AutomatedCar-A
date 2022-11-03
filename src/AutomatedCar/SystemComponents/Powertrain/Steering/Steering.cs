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
    public class Steering : ISteering
    {
        private const double TurningOffset = 5;

        private int wheelBase;
        private double steerAngle;

        private Vector carLocation;
        private double carHeading;
        private int carSpeed;

        private double rotation;

        private Vector frontWheel;
        private Vector backWheel;

        private int reverseMultiplier;
        private GearshiftState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Steering"/> class.
        /// </summary>
        public Steering()
        {
            this.wheelBase = 300;
            this.steerAngle = 0;
            this.carHeading = -1.5;
        }

        /// <summary>
        /// Gets or sets the state of the Gearshift.
        /// </summary>
        public GearshiftState State
        {
            get { return this.state; }
            set { this.state = value; }
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
            get { return this.rotation; }
            set { this.rotation = value; }
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
            this.steerAngle = -TurningOffset;
            this.GetRotation();
        }

        /// <summary>
        /// Turning right.
        /// </summary>
        public void TurnRight()
        {
            this.steerAngle = +TurningOffset;
            this.GetRotation();
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

            this.GetRotation();
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
            this.SetReverseMultiplier();
            this.backWheel += this.carSpeed * new Vector(Math.Cos(this.carHeading), Math.Sin(this.carHeading)) * this.reverseMultiplier;
            this.frontWheel += this.carSpeed * new Vector(Math.Cos(this.carHeading + this.steerAngle), Math.Sin(this.carHeading + this.steerAngle)) * this.reverseMultiplier;
        }

        private void GetNewHeading()
        {
            this.carLocation = (this.frontWheel + this.backWheel) / 2;
            this.carHeading = Math.Atan2(this.frontWheel.Y - this.backWheel.Y, this.frontWheel.X - this.backWheel.X);
        }

        private void SetReverseMultiplier()
        {
            if (this.state == GearshiftState.R)
            {
                this.reverseMultiplier = -1;
            }
            else
            {
                this.reverseMultiplier = 1;
            }
        }
    }
}
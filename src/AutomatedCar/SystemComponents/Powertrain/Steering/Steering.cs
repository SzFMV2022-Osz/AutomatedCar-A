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
        private const int TurningOffset = 5;

        private int wheelBase;
        private double steerAngle;

        private Vector carLocation;
        private double carHeading;
        private float carSpeed;

        private double rotation;

        private Vector frontWheel;
        private Vector backWheel;

        private int reverseMultiplier;
        private GearshiftState state;

        /// <summary>
        /// Initializes a new instance of the <see cref="Steering"/> class.
        /// </summary>
        /// <param name="x">X position of car.</param>
        /// <param name="y">Y position of car.</param>
        /// <param name="carSpeed">Speed of car.</param>
        /// <param name="state">State of the gearshift.</param>
        /// <param name="rotation">Rotation of the car.</param>
        public Steering(int x, int y, float carSpeed, GearshiftState state, double rotation)
        {
            this.wheelBase = 300;
            this.steerAngle = 0;
            this.carLocation = new Vector(x, y);
            this.carSpeed = carSpeed;
            this.carHeading = -1.5;
            this.state = state;
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

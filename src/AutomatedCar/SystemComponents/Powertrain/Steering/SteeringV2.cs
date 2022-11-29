// <copyright file="SteeringV2.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain.Steering
{
    using System;
    using AutomatedCar.Models;

    /// <summary>
    /// SteeringV2.
    /// </summary>
    public class SteeringV2
    {
        private int steerAngle;
        private int steeringSpeed = 5;
        private int wheelBase = 130;
        private double dt = 1; // 1 / 60;

        /// <summary>
        /// Initializes a new instance of the <see cref="SteeringV2"/> class.
        /// </summary>
        public SteeringV2()
        {
            this.steerAngle = 0;
        }

        /// <summary>
        /// Gets the SteeringAngle of the Car.
        /// </summary>
        public int SteeringAngle
        {
            get { return this.steerAngle; }
        }

        /// <summary>
        /// Turning right.
        /// </summary>
        public void SteeringRight()
        {
            if (this.steerAngle < 60)
            {
                this.steerAngle += this.steeringSpeed;
            }
        }

        /// <summary>
        /// Sets the turning direction to the left side.
        /// </summary>
        public void SteeringLeft()
        {
            if (this.steerAngle > -60)
            {
                this.steerAngle -= this.steeringSpeed;
            }
        }

        /// <summary>
        /// Resets the wheel to a straight position.
        /// </summary>
        public void SteeringCenter()
        {
            if (this.steerAngle > 0)
            {
                this.steerAngle -= this.steeringSpeed;
            }

            if (this.steerAngle < 0)
            {
                this.steerAngle += this.steeringSpeed;
            }
        }

        /// <summary>
        /// Calculates the new car position.
        /// </summary>
        /// <param name="carSpeed">Speed value from the engine.</param>
        /// <param name="car">Our current car.</param>
        public void SteeringAndSetNewCarPosition(int carSpeed, AutomatedCar car)
        {
            double normalHeading = this.ConvertAngleFromAvalonia(car.Rotation);
            double normalHeadingRad = this.ToRadian(normalHeading);
            double steerAngleRad = this.ToRadian(this.steerAngle);

            double frontWheelX = car.X + (this.wheelBase / 2 * Math.Cos(normalHeadingRad));
            double frontWheelY = car.Y + (this.wheelBase / 2 * Math.Sin(normalHeadingRad));
            double backWheelX = car.X - (this.wheelBase / 2 * Math.Cos(normalHeadingRad));
            double backWheelY = car.Y - (this.wheelBase / 2 * Math.Sin(normalHeadingRad));

            /*if (carSpeed != 0)
            {
                carSpeed = (50 / (1 / (carSpeed * 1000 / 60 / 60 / 60)));
            }*/

            backWheelX += (carSpeed * 1000 / 60 / 60) * this.dt * Math.Cos(normalHeadingRad);
            backWheelY += (carSpeed * 1000 / 60 / 60) * this.dt * Math.Sin(normalHeadingRad);
            frontWheelX += (carSpeed * 1000 / 60 / 60) * this.dt * Math.Cos(normalHeadingRad + steerAngleRad);
            frontWheelY += (carSpeed * 1000 / 60 / 60) * this.dt * Math.Sin(normalHeadingRad + steerAngleRad);

            car.X = Convert.ToInt32((frontWheelX + backWheelX) / 2);
            car.Y = Convert.ToInt32((frontWheelY + backWheelY) / 2);

            normalHeading = this.ToDegree(Math.Atan2(frontWheelY - backWheelY, frontWheelX - backWheelX));
            car.Rotation = this.ConvertAngleToAvalonia(normalHeading);
        }

        /// <summary>
        /// Degree recalculation.
        /// </summary>
        /// <param name="degree">The angle of the car from the ui.</param>
        /// <returns>Gives us back an angle we can work with.</returns>
        private double ConvertAngleFromAvalonia(double degree)
        {
            degree -= 90;
            degree = this.NormalizeDegree(degree);

            return degree;
        }

        /// <summary>
        /// Degree recalculation.
        /// </summary>
        /// <param name="degree">The angle we worked with.</param>
        /// <returns>The angle of the car from the ui.</returns>
        private double ConvertAngleToAvalonia(double degree)
        {
            degree += 90;
            degree = this.NormalizeDegree(degree);

            return degree;
        }

        /// <summary>
        /// If the degree is not between 0 and 360, this will help.
        /// </summary>
        /// <param name="val">degree value.</param>
        /// <returns>Return a corrected value if it was necessary.</returns>
        private double NormalizeDegree(double val)
        {
            if (val < 0)
            {
                val += 360;
            }

            if (val > 360)
            {
                val -= 360;
            }

            return val;
        }

        /// <summary>
        /// Converts degree to radian.
        /// </summary>
        /// <param name="degree">Value of degree.</param>
        /// <returns>Radian value of degree.</returns>
        private double ToRadian(double degree)
        {
            return (degree / 180) * Math.PI;
        }

        /// <summary>
        /// Converts radian to degree.
        /// </summary>
        /// <param name="radian">Value of radian degree.</param>
        /// <returns>degree.</returns>
        private double ToDegree(double radian)
        {
            return (radian / Math.PI) * 180;
        }
    }
}

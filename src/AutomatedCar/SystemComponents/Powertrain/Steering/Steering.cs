// <copyright file="Steering.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;
    using Vector = Avalonia.Vector;

    /// <summary>
    /// Steering.
    /// </summary>
    internal class Steering
    {
        //atan(wheelBase / (turningCircle - carWidth)) = turningAngle
        float wheelBase;
        double steerAngle;

        Vector carLocation;
        double carHeading;
        float carSpeed;

        Vector frontWheel;
        Vector backWheel;

        float dt;

        public Steering(double steerAngle, int x, int y, float carSpeed)
        {
            this.wheelBase = 130f;
            this.steerAngle = steerAngle;
            this.carLocation = new Vector(x, y);
            this.carSpeed = carSpeed;
        }

        public void findWheelLocations()
        {
            frontWheel = carLocation + wheelBase / 2 * new Vector(Math.Cos(carHeading), Math.Sin(carHeading));
            backWheel = carLocation - wheelBase / 2 * new Vector(Math.Cos(carHeading), Math.Sin(carHeading));
        }

        public void findNewWheelLocations()
        {
            backWheel += carSpeed * dt * new Vector(Math.Cos(carHeading), Math.Sin(carHeading));
            frontWheel += carSpeed * dt * new Vector(Math.Cos(carHeading + steerAngle), Math.Sin(carHeading + steerAngle));
        }

        public void getNewHeading()
        {
            carLocation = (frontWheel + backWheel) / 2;
            carHeading = Math.Atan2(frontWheel.Y - backWheel.Y, frontWheel.X - backWheel.X);
        }
    }
}

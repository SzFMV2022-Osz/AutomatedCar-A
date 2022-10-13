// <copyright file="Engine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engine.
    /// </summary>
    internal class Engine : IEngine
    {
        private static readonly float Pi = (float)Math.PI;
        private readonly IGearshift gearshift;
        private readonly int mass;
        private readonly float frontArea;
        private readonly float wheelradius;
        private readonly float dragCoefficient;
        private readonly float diferential;
        private readonly float transmissionefficiency;
        private float rpm;
        private float maxrpm;
        private float minrpm;
        private float crr;
        private float torque;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="gearshift">Gearshift to use.</param>
        /// <param name="mass">Mass of the car.</param>
        /// <param name="dragCoefficient">Drag coefficient of the car.</param>
        /// <param name="frontArea">Front area of the car.</param>
        /// <param name="diferential">Diferencial ratio of the car.</param>
        /// <param name="transmissionefficiency">Transmission efficiency of the car.</param>
        /// <param name="wheelradius">Wheelradious.</param>
        public Engine(IGearshift gearshift, int mass = 1800, float dragCoefficient = 0.30f, float frontArea = 2.2f, float diferential = 3.42f, float transmissionefficiency = 0.7f, float wheelradius = 0.34f)
        {
            this.gearshift = gearshift;
            this.mass = mass;
            this.dragCoefficient = dragCoefficient;
            this.frontArea = frontArea;
            this.diferential = diferential;
            this.transmissionefficiency = transmissionefficiency;
            this.wheelradius = wheelradius;
            this.crr = 30 * this.Cdrag();
        }

        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        public int Speed { get; private set; }

        /// <summary>
        /// Gets rotation rate of the wheel.
        /// </summary>
        private float WheelRotationRate
        {
            get { return this.GetWheelRotationRateBySpeed(); }
        }

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Accelerate()
        {
            if (!this.rpm.Equals(this.maxrpm))
            {
                if (this.GetNextGearRPM() > this.minrpm)
                {
                    this.gearshift.ShiftUp();
                    this.rpm = this.GetRPM();
                }

                this.rpm += 0.5f;
                this.Speed = (int)this.GetSpeedByWheelRotation();
            }

            return this.DrivingForce() - this.DragForce();
        }

        /// <summary>
        /// Slows the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Decelerate()
        {
            if (!this.rpm.Equals(this.minrpm))
            {
                if (this.GetPrewGearRPM() < this.maxrpm)
                {
                    this.gearshift.ShiftDown();
                    this.rpm = this.GetRPM();
                }

                this.rpm -= 0.5f;
                this.Speed = (int)this.GetSpeedByWheelRotation();
            }

            return this.DrivingForce() - this.DragForce();
        }

        private int GetRPM()
        {
            return (int)(this.WheelRotationRate * this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private int GetNextGearRPM()
        {
            return (int)(this.WheelRotationRate * this.gearshift.NextGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private int GetPrewGearRPM()
        {
            return (int)(this.WheelRotationRate * this.gearshift.PreviousGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateByRPM()
        {
            return this.rpm / (this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateBySpeed()
        {
            return this.Speed / this.wheelradius;
        }

        private float GetSpeedByWheelRotation()
        {
            return this.GetWheelRotationRateByRPM() * this.wheelradius;
        }

        private float Cdrag()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f;
        }

        private float DragForce()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f * (this.Speed * this.Speed);
        }

        private float DrivingForce()
        {
            return this.torque * this.gearshift.GetGearRatio() * this.diferential * this.transmissionefficiency / this.wheelradius;
        }
    }
}

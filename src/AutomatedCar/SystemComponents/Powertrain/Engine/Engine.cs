// <copyright file="Engine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
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
        private readonly float cBreak;
        private readonly float maxrpm;
        private readonly float minrpm;
        private float rpm;
        private float gasPedal;
        private float breakPedal;

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
        /// <param name="maxrpm">Maximum rpm.</param>
        /// <param name="minrpm">Minimum rpm.</param>
        /// <param name="cBreak">Max break force rpm.</param>
        public Engine(IGearshift gearshift, int mass = 1800, float dragCoefficient = 0.30f, float frontArea = 2.2f, float diferential = 3.42f, float transmissionefficiency = 0.7f, float wheelradius = 0.34f, float maxrpm = 6000, int minrpm = 1000, int cBreak = 100)
        {
            this.gearshift = gearshift;
            this.mass = mass;
            this.dragCoefficient = dragCoefficient;
            this.frontArea = frontArea;
            this.diferential = diferential;
            this.transmissionefficiency = transmissionefficiency;
            this.wheelradius = wheelradius;
            this.maxrpm = maxrpm;
            this.minrpm = minrpm;
            this.cBreak = cBreak;
            this.Velocity = 0;
        }

        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        public int Speed
        {
            get { return (int)(this.Velocity * 3.6); }
        }

        private float Velocity { get; set; }

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Accelerate()
        {
            if (this.gearshift.GetGearRatio() == 0)
            {
                this.gearshift.ShiftUp();
                this.rpm = this.minrpm;
            }

            this.gasPedal += .01f;
            this.breakPedal -= .01f;
            this.ClampPedals();
            this.Velocity += this.ChangeVelocity(false) + this.ChangeVelocity(true);
            this.rpm = this.GetRPM();

            if (this.rpm.Equals(this.maxrpm))
            {
                if (this.GetNextGearRPM() > this.minrpm)
                {
                    this.gearshift.ShiftUp();
                    this.rpm = this.GetRPM();
                }
            }
            else if (this.rpm < this.minrpm)
            {
                this.rpm = this.minrpm;
            }

            return this.LongitudinalForce();
        }

        /// <summary>
        /// Slows the car with enginebreak.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Lift()
        {
            this.rpm -= 0.25f; // enginebreak
            this.breakPedal -= .01f;
            this.gasPedal -= .01f;
            this.ClampPedals();
            this.Velocity = (int)this.GetSpeedByWheelRotation() + this.ChangeVelocity(false) + this.ChangeVelocity(true);
            if (this.rpm < this.minrpm + 0.25f)
            {
                if (this.GetPrewGearRPM() < this.maxrpm)
                {
                    this.gearshift.ShiftDown();
                    if (this.gearshift.GetGearRatio() == 0)
                    {
                        this.rpm = this.minrpm;
                    }
                    else
                    {
                        this.rpm = this.GetRPM();
                    }
                }
            }

            return this.LongitudinalForce(true);
        }

        /// <summary>
        /// Slows the car with breaks.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Breaking()
        {
            this.breakPedal += .01f;
            this.gasPedal -= .01f;
            this.ClampPedals();
            this.Velocity += this.ChangeVelocity(false) + this.ChangeVelocity(true);
            this.rpm = this.GetRPM();

            if (this.rpm < this.minrpm)
            {
                if (this.GetPrewGearRPM() < this.maxrpm)
                {
                    this.gearshift.ShiftDown();
                    if (this.gearshift.GetGearRatio() == 0)
                    {
                        this.rpm = this.minrpm;
                    }
                    else
                    {
                        this.rpm = this.GetRPM();
                    }
                }
            }

            return this.LongitudinalForce();
        }

        /// <summary>
        /// Switch state up.
        /// </summary>
        public void StateDown()
        {
            this.gearshift.StateDown();
        }

        /// <summary>
        /// Switch state dawn.
        /// </summary>
        public void StateUp()
        {
            this.gearshift.StateUp();
        }

        private int GetRPM()
        {
            return (int)(this.GetWheelRotationRateBySpeed() * this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private int GetNextGearRPM()
        {
            return (int)(this.GetWheelRotationRateBySpeed() * this.gearshift.NextGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private int GetPrewGearRPM()
        {
            return (int)(this.GetWheelRotationRateBySpeed() * this.gearshift.PreviousGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateByRPM()
        {
            return this.rpm / (this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateBySpeed()
        {
            return this.Velocity / this.wheelradius;
        }

        private float GetSpeedByWheelRotation()
        {
            float speed = 0;
            switch (this.gearshift.GetState())
            {
                case GearshiftState.P:
                    speed = 0;
                    break;
                case GearshiftState.R:
                    speed = -1 * this.GetWheelRotationRateByRPM() * this.wheelradius;
                    break;
                case GearshiftState.N:
                    speed = 0;
                    break;
                case GearshiftState.D:
                    speed = this.GetWheelRotationRateByRPM() * this.wheelradius;
                    break;
            }

            return speed;
        }

        private float Cdrag()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f;
        }

        private float Crr()
        {
            return 30.0f * this.Cdrag();
        }

        private float DragForce()
        {
            return 0.5f * this.Cdrag() * this.frontArea * 1.29f * (this.Velocity * this.Velocity);
        }

        private float DrivingForce()
        {
            var a = this.GetTorque();

            return this.GetTorque() * this.gearshift.GetGearRatio() * this.diferential * this.transmissionefficiency / this.wheelradius;
        }

        private float LongitudinalForce(bool isbreaking = false)
        {
            float temp = 0.0f;
            switch (this.gearshift.GetState())
            {
                case GearshiftState.P:
                    break;
                case GearshiftState.R:
                    if (isbreaking)
                    {
                        temp = this.BreakingForce() + this.DragForce() + this.Frr();
                    }
                    else
                    {
                        temp = this.DrivingForce() + this.DragForce() + this.Frr();
                    }

                    temp *= -1;
                    break;
                case GearshiftState.N:
                    break;
                case GearshiftState.D:
                    if (isbreaking)
                    {
                        temp = this.BreakingForce() + this.DragForce() + this.Frr();
                    }
                    else
                    {
                        float a = this.Frr();
                        float b = this.DragForce();
                        float c = this.DrivingForce();
                        temp = this.DrivingForce() + this.DragForce() + this.Frr();
                    }

                    break;
            }

            return temp;
        }

        private float BreakingForce()
        {
            return this.cBreak * this.breakPedal * -1;
        }

        private float ChangeVelocity(bool isbreaking)
        {
            return this.LongitudinalForce(isbreaking) / this.mass;
        }

        private float GetTorque()
        {
            return this.gasPedal * this.LookupTorqueCurve(this.rpm);
        }

        private float LookupTorqueCurve(float rpm)
        {
            return this.LookupHpCurve(rpm) * 5252 / rpm;
        }

        private float LookupHpCurve(float rpm)
        {
            if (rpm >= 1000 && rpm <= 5000)
            {
                return (0.06875f * rpm) - 18.75f;
            }
            else if (rpm > 5000 && rpm < this.maxrpm)
            {
                return (-1 * (float)Math.Pow(rpm - 5500, 2) * 0.000099f) + 350;
            }
            else
            {
                return 325.25f;
            }
        }

        private float Frr()
        {
            return -1 * this.Crr() * this.Velocity;
        }

        private void ClampPedals()
        {
            if (this.gasPedal > 1.0f)
            {
                this.gasPedal = 1.0f;
            }
            else if (this.gasPedal < 0.0f)
            {
                this.gasPedal = 0;
            }

            if (this.breakPedal > 1.0f)
            {
                this.breakPedal = 1.0f;
            }
            else if (this.breakPedal < 0.0f)
            {
                this.breakPedal = 0;
            }
        }
    }
}

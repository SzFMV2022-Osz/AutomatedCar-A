// <copyright file="Engine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Engine.
    /// </summary>
    public class Engine : IEngine
    {
        private static readonly float Pi = (float)Math.PI;
        private readonly IGearshift gearshift;
        private readonly int mass;
        private readonly float frontArea;
        private readonly float wheelradius;
        private readonly float dragCoefficient;
        private readonly float diferential;
        private readonly float transmissionefficiency;
        private readonly float cBrake;
        private readonly float maxrpm;
        private readonly float minrpm;
        private float rpm;
        private float gasPedal;
        private float brakePedal;
        private float velocity;

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
        /// <param name="cbrake">Max brake force rpm.</param>
        public Engine(IGearshift gearshift, int mass = 1800, float dragCoefficient = 0.30f, float frontArea = 2.2f, float diferential = 3.42f, float transmissionefficiency = 0.7f, float wheelradius = 0.34f, float maxrpm = 6000, int minrpm = 1000, int cbrake = 100)
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
            this.cBrake = cbrake;
            this.rpm = 1000;
            this.velocity = 0;
        }

        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        public int GetSpeed
        {
            get { return (int)(this.Velocity / 3.6); }
        }

        /// <summary>
        /// Gets RPM of the car.
        /// </summary>
        public int GetRPMValue
        {
            get
            {
                return (int)this.rpm;
            }
        }

        /// <summary>
        /// Gets the state of the gearbox. Only for debug purposes.
        /// </summary>
        public GearshiftState GetGearshiftState
        {
            get
            {
                return this.gearshift.GetState();
            }
        }

        /// <summary>
        /// Gets the percentage value of the throttle.
        /// </summary>
        public int GetThrottleValue
        {
            get
            {
                return (int)this.gasPedal;
            }
        }

        /// <summary>
        /// Gets the percentage value of the brake.
        /// </summary>
        public int GetBrakeValue
        {
            get
            {
                return (int)0;
            }
        }

        private float Velocity
        {
            get
            {
                return this.velocity;
            }

            set
            {
                this.velocity = value;
            }
        }

        /// <summary>
        /// Accelerates the car by adjusting gas and brake pedals, also calculates the velocity.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Accelerate()
        {
            this.gasPedal += .01f;
            this.brakePedal -= .01f;
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

            return this.LongitudinalForce();
        }

        /// <summary>
        /// Slows down the car by the engine brake.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Lift()
        {
            this.rpm -= 0.75f; // enginebrake
            this.brakePedal -= .25f;
            this.gasPedal -= .25f;
            this.Velocity = (int)this.GetSpeedByWheelRotation() + this.ChangeVelocity(false) + this.ChangeVelocity(true);
            if (this.rpm < this.minrpm + 0.25f)
            {
                if (this.GetPrewGearRPM() < this.maxrpm)
                {
                    this.gearshift.ShiftDown();
                    this.rpm = this.GetRPM();
                }
            }

            return this.LongitudinalForce(true);
        }

        /// <summary>
        /// Slows down the car by adjusting the gas and brake pedals, also calculates the new velocity.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        public float Braking()
        {
            this.brakePedal += .01f;
            this.gasPedal -= .01f;
            this.ClampPedals();
            this.Velocity += this.ChangeVelocity(false) + this.ChangeVelocity(true);
            this.rpm = this.GetRPM();

            if (this.rpm < this.minrpm)
            {
                if (this.GetPrewGearRPM() < this.maxrpm)
                {
                    this.gearshift.ShiftDown();
                    this.rpm = this.GetRPM();
                }
            }

            return this.LongitudinalForce();
        }

        /// <summary>
        /// Puts the gearbox state into a lower level.
        /// </summary>
        public void StateDown()
        {
            this.gearshift.StateDown();
        }

        /// <summary>
        /// Puts the gearbox state into a upper level.
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
            return Math.Abs(this.Velocity / this.wheelradius);
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
            return this.GetTorque() * this.gearshift.GetGearRatio() * this.diferential * this.transmissionefficiency / this.wheelradius;
        }

        private float LongitudinalForce(bool isbraking = false)
        {
            float temp = 0.0f;
            switch (this.gearshift.GetState())
            {
                case GearshiftState.P:
                    break;
                case GearshiftState.R:
                    if (isbraking)
                    {
                        temp = this.BrakingForce() + this.DragForce() + this.Frr();
                        if(brakePedal > 0)
                        {
                            Debug.WriteLine("Brake pedal percentage: " + brakePedal);
                        }
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
                    if (isbraking)
                    {
                        temp = this.BrakingForce() + this.DragForce() + this.Frr();
                    }
                    else
                    {
                        temp = this.DrivingForce() + this.DragForce() + this.Frr();
                    }

                    break;
            }

            return temp;
        }

        private float BrakingForce()
        {
            return this.cBrake * this.brakePedal * -1;
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

            if (this.brakePedal > 1.0f)
            {
                this.brakePedal = 1.0f;
            }
            else if (this.brakePedal < 0.0f)
            {
                this.brakePedal = 0;
            }
        }
    }
}

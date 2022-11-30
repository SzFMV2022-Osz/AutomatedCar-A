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
        private readonly int weight = 1800;
        private readonly float frontArea = 2.2f;
        private readonly float wheelradius = 0.34f;
        private readonly float dragCoefficient = 0.30f;
        private readonly float diferential = 3.42f;
        private readonly float transmissionefficiency = 0.7f;
        private readonly float brakeConstant = 100;
        private readonly float maxrpm = 6000;
        private readonly float minrpm = 1000;

        private int[] torqueCurve = new int[7] { 400, 430, 450, 480, 460, 450, 390 };
        private float rpm;
        private float gasPedal;
        private float brakePedal;
        private float velocity;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="bus">A virtualfunctionbus.</param>
        /// <param name="gearshift">Gearshift to use.</param>
        /// <param name="weight">weight of the car.</param>
        /// <param name="dragCoefficient">Drag coefficient of the car.</param>
        /// <param name="frontArea">Front area of the car.</param>
        /// <param name="diferential">Diferencial ratio of the car.</param>
        /// <param name="transmissionefficiency">Transmission efficiency of the car.</param>
        /// <param name="wheelradius">Wheelradious.</param>
        /// <param name="maxrpm">Maximum rpm.</param>
        /// <param name="minrpm">Minimum rpm.</param>
        /// <param name="cbrake">Max brake force rpm.</param>
        public Engine(IGearshift gearshift)
        {
            this.gearshift = gearshift;
            this.rpm = 1000;
            this.velocity = 0;
            this.brakePedal = 0;
            this.gasPedal = 0;
        }

        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        public int GetSpeed
        {
            get { return (int)(this.Velocity * 3.6); }
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
        public float GetThrottleValue
        {
            get
            {
                return this.gasPedal;
            }
        }

        /// <summary>
        /// Gets the percentage value of the brake.
        /// </summary>
        public float GetBrakeValue
        {
            get
            {
                return this.brakePedal;
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

        /// <summary>
        /// Accelerates the car by adjusting gas and brake pedals, also calculates the velocity.
        /// </summary>
        public void Accelerate()
        {
            this.gasPedal += .01f;
            if (this.gasPedal > 1.0f)
            {
                this.gasPedal = 1.0f;
            }
            else if (this.gasPedal < 0.0f)
            {
                this.gasPedal = 0;
            }

            if (this.brakePedal > 0)
            {
                this.brakePedal -= .01f;
            }
            else
            {
                this.brakePedal = 0;
            }

            this.Velocity = this.GetSpeedByWheelRotation() + this.ChangeVelocity(false);
            this.rpm = this.GetRPM();

            /*Debug.WriteLine("Gas: " + this.gasPedal);
            Debug.WriteLine("Brake: " + this.brakePedal);*/
            Debug.WriteLine("RPM: " + this.rpm);

            /*if (this.rpm.Equals(this.maxrpm))
            {
                if (this.GetNextGearRPM() > this.minrpm)
                {
                    this.gearshift.ShiftUp();
                    this.rpm = this.GetRPM();
                }
            }*/

            if (this.rpm >= this.maxrpm)
            {
                this.gearshift.ShiftUp();
                this.rpm = this.GetRPM();
            }
        }

        /// <summary>
        /// Slows down the car by the engine brake.
        /// </summary>
        public void Lift()
        {
            this.rpm -= 1f; // enginebrake

            if (this.gasPedal > 0)
            {
                this.gasPedal -= 0.25f;
            }
            else
            {
                this.gasPedal = 0;
            }

            if (this.brakePedal > 0)
            {
                this.brakePedal -= 0.25f;
            }
            else
            {
                this.brakePedal = 0;
            }

            this.Velocity += this.ChangeVelocity(false) + this.ChangeVelocity(true);

            /*Debug.WriteLine("Gas: " + this.gasPedal);
            Debug.WriteLine("Brake: " + this.brakePedal);*/
            Debug.WriteLine("RPM: " + this.rpm);

            if (this.rpm <= this.minrpm)
            {
                this.gearshift.ShiftDown();
                this.rpm = this.GetRPM();
            }
        }

        /// <summary>
        /// Slows down the car by adjusting the gas and brake pedals, also calculates the new velocity.
        /// </summary>
        public void Braking()
        {
            this.brakePedal += .01f;
            if (this.brakePedal > 1.0f)
            {
                this.brakePedal = 1.0f;
            }
            else if (this.brakePedal < 0.0f)
            {
                this.brakePedal = 0;
            }

            if (this.gasPedal > 0)
            {
                this.gasPedal -= .01f;
            }
            else
            {
                this.brakePedal = 0;
            }

            this.Velocity += this.ChangeVelocity(true);
            this.rpm = this.GetRPM();

            /*Debug.WriteLine("Gas: " + this.gasPedal);
            Debug.WriteLine("Brake: " + this.brakePedal);*/
            Debug.WriteLine("RPM: " + this.rpm);

            if (this.rpm <= this.minrpm)
            {
                this.gearshift.ShiftDown();
                this.rpm = this.GetRPM();
            }
        }

        private int GetSpeedByWheelRotation()
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
                    if (this.gearshift.GetPrevState() == GearshiftState.R)
                    {
                        speed = -1 * this.GetWheelRotationRateByRPM() * this.wheelradius;
                    }
                    else if (this.gearshift.GetPrevState() == GearshiftState.D)
                    {
                        speed = this.GetWheelRotationRateByRPM() * this.wheelradius;
                    }
                    else
                    {
                        speed = 0;
                    }

                    break;
                case GearshiftState.D:
                    speed = this.GetWheelRotationRateByRPM() * this.wheelradius;
                    break;
            }

            return (int)speed;
        }

        private float GetWheelRotationRateByRPM()
        {
            return this.rpm / (this.gearshift.GetGearRatio() * this.diferential * 60 / (2 * Pi));
        }

        private float ChangeVelocity(bool isbraking = false)
        {
            float longitudionalForce = 0.0f;
            switch (this.gearshift.GetState())
            {
                case GearshiftState.P:
                    break;
                case GearshiftState.R:
                    if (isbraking)
                    {
                        longitudionalForce = this.BrakingForce() + this.DragForce() + this.Frr();
                    }
                    else
                    {
                        longitudionalForce = this.DrivingForce() + this.DragForce() + this.Frr();
                        if (longitudionalForce < 0)
                        {
                            longitudionalForce = 0;
                        }
                    }

                    longitudionalForce *= -1;

                    if (longitudionalForce > 0 && !isbraking)
                    {
                        longitudionalForce = 0;
                    }

                    break;
                case GearshiftState.N:
                    if (isbraking)
                    {
                        longitudionalForce = this.BrakingForce() + this.DragForce() + this.Frr();
                    }

                    break;
                case GearshiftState.D:
                    if (isbraking)
                    {
                        longitudionalForce = this.BrakingForce() + this.DragForce() + this.Frr();
                    }
                    else
                    {
                        longitudionalForce = this.DrivingForce() + this.DragForce() + this.Frr();
                        if (longitudionalForce < 0)
                        {
                            longitudionalForce = 0;
                        }
                    }

                    break;
            }

            return longitudionalForce / this.weight;
        }

        /// <summary>
        /// Calculates the force applied by the pedal and constant.
        /// </summary>
        /// <returns>Returns th braking force.</returns>
        private float BrakingForce()
        {
            return this.brakeConstant * this.brakePedal * -1;
        }

        private float DrivingForce()
        {
            return this.GetTorque() * this.gearshift.GetGearRatio() * this.diferential * this.transmissionefficiency / this.wheelradius;
        }

        private float DragForce()
        {
            float dragConstant = 0.5f * this.dragCoefficient * this.frontArea * 1.29f;

            return -1 * dragConstant * this.frontArea * 1.29f * (this.Velocity * this.Velocity);
        }

        private float Frr()
        {
            float dragConstant = 0.5f * this.dragCoefficient * this.frontArea * 1.29f;
            float crr = 30.0f * dragConstant;

            return -1 * crr * this.Velocity;
        }

        private int GetRPM()
        {
            float wheelRotation = this.GetWheelRotationRateBySpeed();
            float gearRatio = this.gearshift.GetGearRatio();

            Debug.WriteLine("Wheel rotation: " + wheelRotation);
            Debug.WriteLine("Gear ratio" + gearRatio);

            int rpm = (int)(wheelRotation * gearRatio * this.diferential * 60 / (2 * Pi));
            if (rpm > this.maxrpm)
            {
                return (int)this.maxrpm;
            }
            else
            {
                if (this.GetSpeed == 0)
                {
                    return 1000;
                }
                else
                {
                    return rpm;
                }

                //return rpm;
            }
        }

        private int GetNextGearRPM()
        {
            return (int)(this.GetWheelRotationRateBySpeed() * this.gearshift.NextGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private int GetPrewGearRPM()
        {
            return (int)(this.GetWheelRotationRateBySpeed() * this.gearshift.PreviousGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateBySpeed()
        {
            return Math.Abs(this.Velocity / this.wheelradius);
        }

        private float GetTorque()
        {
            return this.gasPedal * this.LookupTorqueCurve(this.rpm) * 5252 / this.rpm;
        }

        /*private float LookupTorqueCurve(float rpm)
        {
            return this.LookupHpCurve(rpm) * rpm / 5252;
        }*/

        private float LookupTorqueCurve(float rpm)
        {
            /*if (rpm >= 1000 && rpm <= 5000)
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
            }*/

            if (rpm == 1000)
            {
                return this.torqueCurve[0];
            }
            else if (rpm > 1000 && rpm < 2000)
            {
                return this.torqueCurve[1];
            }
            else if (rpm < 3000)
            {
                return this.torqueCurve[2];
            }
            else if (rpm < 4000)
            {
                return this.torqueCurve[3];
            }
            else if (rpm < 5000)
            {
                return this.torqueCurve[4];
            }
            else if (rpm < 5500)
            {
                return this.torqueCurve[5];
            }
            else if (rpm < 6000)
            {
                return this.torqueCurve[6];
            }

            return 0;
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

// <copyright file="Engine.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class RevolutionsHelper
    {
        // Stores a value pair for each inner gear, with the second value representing the rate at which the RPM should increase in the respective gear
        public static List<Tuple<int, double>> GearCoefficients { get; set; } = new List<Tuple<int, double>>()
        {
            new Tuple<int, double>(0, 1), // this is for N
            new Tuple<int, double>(1, 0.8),
            new Tuple<int, double>(2, 0.3),
            new Tuple<int, double>(3, 0.15),
            new Tuple<int, double>(4, 0.1),
        };
    }

    public class Vector : IEquatable<Vector>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Vector()
        {

        }

        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool Equals(Vector other)
        {
            if (other.X == this.X && other.Y == this.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Engine.
    /// </summary>
    public class Engine : IEngine
    {
        private const int PEDAL_OFFSET = 16;
        private const int MIN_PEDAL_POSITION = 0;
        private const int MAX_PEDAL_POSITION = 100;
        private const double PEDAL_INPUT_MULTIPLIER = 0.01;
        private const double DRAG = 0.01;
        private const int IDLE_RPM = 800;
        private const int MAX_RPM = 6000;
        private const int NEUTRAL_RPM_MULTIPLIER = 80;
        private const int RPM_DOWNSHIFT_POINT = 1300;
        private const int RPM_UPSHIFT_POINT = 2500;

        private int gasPedalPosition;
        private int brakePedalPosition;
        private int revolution;

        private Vector velocity;
        private Vector acceleration;

        public Engine()
        {
            this.velocity = new Vector();
            this.acceleration = new Vector();
            this.revolution = IDLE_RPM;
            this.Gearshift = new Gearshift();
        }


        public int GetThrottleValue
        {
            get
            {
                return this.gasPedalPosition;
            }

            private set
            {
                this.gasPedalPosition = value;
            }
        }

        public int GetBrakeValue
        {
            get
            {
                return this.brakePedalPosition;
            }

            private set
            {
                this.brakePedalPosition = value;
            }
        }

        public int GetRPMValue
        {
            get
            {
                return this.revolution;
            }

            set
            {
                this.revolution = value;
            }
        }

        public IGearshift Gearshift { get; set; }

        public Vector Velocity
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

        public Vector Acceleration
        {
            get
            {
                return this.acceleration;
            }

            set
            {
                this.acceleration = value;
            }
        }

        public int GetSpeed { get; set; }

        public void CalculateSpeed()
        {
            this.GetSpeed = (int)Math.Sqrt(Math.Pow(this.Velocity.X, 2) + Math.Pow(this.Velocity.Y, 2));
        }

        /*public void CalculateNextPosition()
        {
            double gasInputForce = this.gasPedalPosition * PEDAL_INPUT_MULTIPLIER;
            double brakeInputForce = this.brakePedalPosition * PEDAL_INPUT_MULTIPLIER;
            double slowingForce = this.GetSpeed * DRAG + (this.GetSpeed > 0 ? brakeInputForce : 0);

            this.Acceleration.Y = gasInputForce;

            this.Velocity.Y = GetVelocityAccordingToGear(slowingForce);

            CalculateSpeed();

            CalculateRevolutions();
            if (Gearshift.InnerShiftingStatus != Shifting.None)
            {
                this.HandleRpmTransitionWhenShifting();
            }
        }*/

        public double GetVelocityAccordingToGear(double slowingForce)
        {
            double velocity = Velocity.Y;

            if (Gearshift.State == GearshiftState.D)
            {
                velocity += -(Acceleration.Y - slowingForce);
            }
            else if (Gearshift.State == GearshiftState.R && GetSpeed < 20)
            {
                velocity += Acceleration.Y - slowingForce;
            }
            else
            {
                if (velocity < 0)
                {
                    velocity += slowingForce;
                }
                else
                {
                    velocity -= slowingForce;
                }
            }

            return velocity;
        }

        public void Accelerate()
        {
            int newPosition = this.gasPedalPosition + PEDAL_OFFSET;
            this.gasPedalPosition = this.BoundPedalPosition(newPosition);
        }

        public void Lift()
        {
            int newPosition = this.gasPedalPosition - PEDAL_OFFSET;
            this.gasPedalPosition = this.BoundPedalPosition(newPosition);
        }

        public void Braking()
        {
            int newPosition = this.brakePedalPosition + PEDAL_OFFSET;
            this.brakePedalPosition = this.BoundPedalPosition(newPosition);
        }

        public void LiftBraking()
        {
            int newPosition = this.brakePedalPosition - PEDAL_OFFSET;
            this.brakePedalPosition = this.BoundPedalPosition(newPosition);
        }

        public void CalculateRevolutions()
        {
            if (this.gasPedalPosition > 0)
            {
                this.IncreaseRevolutions();
            }
            else
            {
                this.DecreaseRevolutions();
            }
        }

        private void IncreaseRevolutions()
        {
            double revolutionsIncreaseRate =
                RevolutionsHelper.GearCoefficients.FirstOrDefault(x => x.Item1 == (int)this.Gearshift.InnerShiftingStatus).Item2;

            if (this.Gearshift.InnerShiftingStatus > 0 && this.revolution < MAX_RPM)
            {
                this.revolution += (int)Math.Round(this.GetSpeed * revolutionsIncreaseRate);
            }
            else if (this.Gearshift.InnerShiftingStatus == 0 && this.revolution < MAX_RPM)
            {
                this.revolution += (int)Math.Round(revolutionsIncreaseRate * NEUTRAL_RPM_MULTIPLIER);
            }

            if (this.revolution > RPM_UPSHIFT_POINT && this.Gearshift.InnerShiftingStatus != 0)
            {
                this.Gearshift.ShiftUp();
            }
        }

        private void DecreaseRevolutions()
        {
            double revolutionsDecreaseRate =
               0.15 / RevolutionsHelper.GearCoefficients.FirstOrDefault(x => x.Item1 == (int)this.Gearshift.InnerShiftingStatus).Item2;
            var revolutionChange = this.brakePedalPosition > 0
                ? this.brakePedalPosition * revolutionsDecreaseRate
                : Math.Pow(Math.Log(this.GetSpeed + 1) / 20, -1.38) * revolutionsDecreaseRate;
            int newRPM = this.revolution - (int)Math.Round(revolutionChange);
            this.revolution = Math.Max(newRPM, IDLE_RPM);

            if (this.revolution < RPM_DOWNSHIFT_POINT && (int)Gearshift.InnerShiftingStatus > 1)
            {
                Gearshift.ShiftDown();
            }
        }

        public void HandleRpmTransitionWhenShifting()
        {
            if (Gearshift.InnerShiftingStatus == Shifting.Up)
            {
                this.revolution -= 100;
                if (this.revolution < 1400)
                {
                    Gearshift.InnerShiftingStatus = Shifting.None;
                }
            }

            if (Gearshift.InnerShiftingStatus == Shifting.Down)
            {
                this.revolution += 100;
                if (this.revolution > 2000)
                {
                    Gearshift.InnerShiftingStatus = Shifting.None;
                }
            }
        }

        private int BoundPedalPosition(int number)
        {
            return Math.Max(MIN_PEDAL_POSITION, Math.Min(number, MAX_PEDAL_POSITION));
        }
    }
}

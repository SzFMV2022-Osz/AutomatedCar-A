// <copyright file="Gearshift.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;

    public enum Shifting { Down, None, Up }

    /// <summary>
    /// Gear shift.
    /// </summary>
    internal class Gearshift : IGearshift
    {
        private GearshiftState state;
        private int currentInternalGear;

        public Shifting InnerShiftingStatus { get; set; } = Shifting.None;

        public Gearshift()
        {
            this.state = GearshiftState.P;
            this.currentInternalGear = 0;
        }

        public GearshiftState State
        {
            get => this.state;

            set => this.state = value;
        }

        public int CurrentInternalGear
        {
            get => this.currentInternalGear;
            set => this.currentInternalGear = value;
        }

        public void StateUp(int velocity, int speed)
        {
            if (state != GearshiftState.D)
            {
                if ((velocity <= 0 && state == GearshiftState.N) || speed == 0 || state == GearshiftState.R)
                {
                    State += 1;

                    if (state == GearshiftState.D)
                    {
                        CurrentInternalGear = 1;
                    }
                }
            }
        }

        public void StateDown(int velocity, int speed)
        {
            if (state != GearshiftState.P)
            {
                if ((velocity >= 0 && state == GearshiftState.N) || speed == 0 || state == GearshiftState.D)
                {
                    State -= 1;

                    if (state == GearshiftState.N)
                    {
                        CurrentInternalGear = 0;
                    }
                }
            }
        }

        public void ShiftDown()
        {
            if (state == GearshiftState.D && currentInternalGear > 0 || state == GearshiftState.N)
            {
                CurrentInternalGear = Math.Max(currentInternalGear - 1, 1);
                InnerShiftingStatus = Shifting.Down;
            }
        }

        public void ShiftUp()
        {
            if (state == GearshiftState.D && currentInternalGear < 4)
            {
                CurrentInternalGear = Math.Min(currentInternalGear + 1, 4);
                InnerShiftingStatus = Shifting.Up;
            }
        }
    }
}
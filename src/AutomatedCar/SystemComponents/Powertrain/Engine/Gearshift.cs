// <copyright file="Gearshift.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    /// <summary>
    /// Gear shift.
    /// </summary>
    internal class Gearshift : IGearshift
    {
        private readonly float[] ratios = { 2.66f, 1.78f, 1.3f, 1f, 0.74f, 0.5f };
        private GearshiftState state;
        private int gear;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gearshift"/> class.
        /// Gear shift constuctor.
        /// </summary>
        public Gearshift()
        {
            this.gear = 1;
            this.state = GearshiftState.P;
        }

        /// <summary>
        /// Returns gear ratio.
        /// </summary>
        /// <returns>ratio.</returns>
        public float GetGearRatio()
        {
            return this.ratios[this.gear];
        }

        /// <summary>
        /// Returns state.
        /// </summary>
        /// <returns>state.</returns>
        public GearshiftState GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Returns next gear ratio.
        /// </summary>
        /// <returns>ratio or -1f.</returns>
        public float NextGearRatio()
        {
            if (this.gear == (this.ratios.Length - 1))
            {
                return -1f;
            }
            else
            {
                return this.ratios[this.gear + 1];
            }
        }

        /// <summary>
        /// Returns previous gear ratio.
        /// </summary>
        /// <returns>ratio or -1f.</returns>
        public float PreviousGearRatio()
        {
            if (this.gear == 0)
            {
                return -1f;
            }
            else
            {
                return this.ratios[this.gear - 1];
            }
        }

        /// <summary>
        /// Sets gear shift state.
        /// </summary>
        /// <param name="state">state.</param>
        public void SetState(GearshiftState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Shifts down.
        /// </summary>
        public void ShiftDown()
        {
            if (this.gear > 0)
            {
                this.gear--;
            }
        }

        /// <summary>
        /// Shifts up.
        /// </summary>
        public void ShiftUp()
        {
            if (this.gear < (this.ratios.Length - 1))
            {
                this.gear++;
            }
        }

        /// <summary>
        /// Switch state dawn.
        /// </summary>
        public void StateDown()
        {
            if (this.state != GearshiftState.P)
            {
                this.state--;
            }
        }

        /// <summary>
        /// Switch state up.
        /// </summary>
        public void StateUp()
        {
            if (this.state != GearshiftState.D)
            {
                this.state++;
            }
        }
    }
}

﻿namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Gear shift.
    /// </summary>
    internal class Gearshift : IGearshift
    {
        private GearshiftState state;
        private int gear;
        private float[] ratios = { 0f, 2.66f, 1.78f, 1.3f, 1f, 0.74f, 0.5f };

        /// <summary>
        /// Initializes a new instance of the <see cref="Gearshift"/> class.
        /// Gear shift constuctor.
        /// </summary>
        public Gearshift()
        {
            this.gear = 0;
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
            if (this.gear == 6)
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
            if (this.gear < 6)
            {
                this.gear++;
            }
        }
    }
}

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// GearShift interface.
    /// </summary>
    internal interface IGearshift
    {
        /// <summary>
        /// Returns next gear ratio.
        /// </summary>
        /// <returns>ratio or -1f.</returns>
        float NextGearRatio();

        /// <summary>
        /// Returns previous gear ratio.
        /// </summary>
        /// <returns>ratio or -1f.</returns>
        float PreviousGearRatio();

        /// <summary>
        /// Shifts up.
        /// </summary>
        void ShiftUp();

        /// <summary>
        /// Shifts down.
        /// </summary>
        void ShiftDown();

        /// <summary>
        /// Sets gear shift state.
        /// </summary>
        /// <param name="state">state.</param>
        void SetState(GearshiftState state);

        /// <summary>
        /// Returns gear ratio.
        /// </summary>
        /// <returns>ratio.</returns>
        float GetGearRatio();

        /// <summary>
        /// Returns state.
        /// </summary>
        /// <returns>state.</returns>
        GearshiftState GetState();
    }
}

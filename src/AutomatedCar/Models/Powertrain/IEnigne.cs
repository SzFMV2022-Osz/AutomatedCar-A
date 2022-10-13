namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engine interface.
    /// </summary>
    internal interface IEnigne
    {
        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        int Speed { get; }

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        float Accelerate();


        /// <summary>
        /// Slows the car.
        /// </summary>
        /// <returns>driving force lenght.</returns>
        float Decelerate();
    }
}

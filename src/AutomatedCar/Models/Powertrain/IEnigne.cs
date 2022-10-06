namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal interface IEnigne
    {
        int Speed { get; }

        void Accelerate();

        void Decelerate();
    }
}

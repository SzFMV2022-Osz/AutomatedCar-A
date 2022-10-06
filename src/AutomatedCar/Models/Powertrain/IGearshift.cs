namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal interface IGearshift
    {
        void ShiftUp();
        void ShiftDown();
        void SetState(GearshiftState state);
        float GetGearRatio();
        GearshiftState GetState();
    }
}

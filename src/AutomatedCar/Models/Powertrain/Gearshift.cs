namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Gearshift : IGearshift
    {
        private GearshiftState state;
        private int gear;
        private float[] ratios = { 0f, 2.66f, 1.78f, 1.3f, 1f, 0.74f, 0.5f };

        public Gearshift()
        {
            this.gear = 0;
            this.state = GearshiftState.P;
        }

        public float GetGearRatio()
        {
            return ratios[gear];
        }

        public GearshiftState GetState()
        {
            return state;
        }

        public void SetState(GearshiftState state)
        {
            this.state = state;
        }

        public void ShiftDown()
        {
            if(gear > 0)
            {
                gear--;
            }
        }

        public void ShiftUp()
        {
            if(gear < 6)
            {
                gear++;
            }
        }
    }
}

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Engine : IEnigne
    {
        public int Speed { get; private set; }
        private int mass;
        private float dragCoefficient;
        private float frontArea;
        private float crr;
        private float torque;
        private IGearshift gearshift;
        public Engine()
        {
            this.crr = 30 * this.Drag();
        }

        public void Accelerate()
        {

        }

        public void Decelerate()
        {

        }

        private float Drag()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f;
        }

        private float DrivingForce()
        {
            return 0.0f;
        }
    }
}

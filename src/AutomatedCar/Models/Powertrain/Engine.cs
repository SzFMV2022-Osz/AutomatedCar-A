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
        private float rpm;
        private float frontArea;
        private float crr;
        private float torque;
        private float diferential;
        private float transmissionefficiency;
        private float wheelradius;
        private float wheelrotationrate;
        private IGearshift gearshift;
        private static float pi = (float)Math.PI;

        public Engine(int mass, float dragCoefficient = 0.30f, float frontArea = 2.2f, float diferential = 3.42f, float transmissionefficiency = 0.7f, float wheelradius = 0.34f)
        {
            this.mass = mass;
            this.dragCoefficient = dragCoefficient;
            this.frontArea = frontArea;
            this.diferential = diferential;
            this.transmissionefficiency = transmissionefficiency;
            this.wheelradius = wheelradius;
            this.crr = 30 * this.Cdrag();
        }

        public void Accelerate()
        {

        }

        public void Decelerate()
        {

        }

        private int getRPM()
        {
            return (int)(this.wheelrotationrate * this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * pi);
        }

        private float GetWheelRotationRateByRPM()
        {
            return this.rpm / (this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * pi);
        }

        private float GetWheelRotationRateBySpeed()
        {
            return this.Speed / this.wheelradius;
        }

        private float Cdrag()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f;
        }

        private float DragForce()
        {
            return 0.5f * this.dragCoefficient * this.frontArea * 1.29f * (this.Speed * this.Speed);
        }

        private float DrivingForce()
        {
            return this.torque * this.gearshift.GetGearRatio() * this.diferential * this.transmissionefficiency / this.wheelradius;
        }
    }
}

namespace AutomatedCar.Models.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engine.
    /// </summary>
    internal class Engine : IEnigne
    {
        private static readonly float Pi = (float)Math.PI;
        private readonly int mass; private float dragCoefficient;
        private float rpm;
        private float maxrpm;
        private float minrpm;
        private float frontArea;
        private float crr;
        private float torque;
        private float diferential;
        private float transmissionefficiency;
        private float wheelradius;
        private float wheelrotationrate;
        private IGearshift gearshift;

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="gearshift">Gearshift to use.</param>
        /// <param name="mass">Mass of the car.</param>
        /// <param name="dragCoefficient">Drag coefficient of the car.</param>
        /// <param name="frontArea">Front area of the car.</param>
        /// <param name="diferential">Diferencial ratio of the car.</param>
        /// <param name="transmissionefficiency">Transmission efficiency of the car.</param>
        /// <param name="wheelradius">Wheelradious.</param>
        public Engine(IGearshift gearshift, int mass, float dragCoefficient = 0.30f, float frontArea = 2.2f, float diferential = 3.42f, float transmissionefficiency = 0.7f, float wheelradius = 0.34f)
        {
            this.gearshift = gearshift;
            this.mass = mass;
            this.dragCoefficient = dragCoefficient;
            this.frontArea = frontArea;
            this.diferential = diferential;
            this.transmissionefficiency = transmissionefficiency;
            this.wheelradius = wheelradius;
            this.crr = 30 * this.Cdrag();
        }

        /// <summary>
        /// Gets speed of the car.
        /// </summary>
        public int Speed { get; private set; }

        /// <summary>
        /// Accelerate the car.
        /// </summary>
        public void Accelerate()
        {
            if (this.rpm.Equals(this.maxrpm))
                return;

            if (this.rpm > 5000)
            {
                this.gearshift.ShiftUp();
            }

            this.rpm += 0.5f;
            this.Speed = (int)this.GetSpeedByWheelRotation();
        }

        /// <summary>
        /// Slows the car.
        /// </summary>
        public void Decelerate()
        {
            if (this.rpm.Equals(this.minrpm))
                return;

            if (this.rpm < 1500)
            {
                this.gearshift.ShiftDown();
            }

            this.rpm -= 0.5f;
            this.Speed = (int)this.GetSpeedByWheelRotation();
        }

        private int GetRPM()
        {
            return (int)(this.wheelrotationrate * this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateByRPM()
        {
            return this.rpm / (this.gearshift.GetGearRatio() * this.diferential * 60 / 2 * Pi);
        }

        private float GetWheelRotationRateBySpeed()
        {
            return this.Speed / this.wheelradius;
        }

        private float GetSpeedByWheelRotation()
        {
            return this.GetWheelRotationRateByRPM() * this.wheelradius;
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

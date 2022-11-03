namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Powertrain : IPowertrain
    {
        private IEngine engine;
        private ISteering steering;
        private IGearshift gearshift;

        public Powertrain()
        {
            this.gearshift = new Gearshift();
            this.engine = new Engine();
            this.steering = new Steering();
        }

        public void CalculateNextPosition()
        {
            double gasInputForce = this.engine.GetThrottleValue * 0.01;
            double brakeInputForce = this.engine.GetBrakeValue * 0.01;
            double slowingForce = this.engine.GetSpeed * 0.01 + (this.engine.GetSpeed > 0 ? brakeInputForce : 0);

            this.engine.Acceleration.Y = gasInputForce;

            this.engine.Velocity.Y = this.engine.GetVelocityAccordingToGear(slowingForce);

            this.engine.CalculateSpeed();
            this.steering.GetRotation();
            this.engine.CalculateRevolutions();
            if (this.gearshift.InnerShiftingStatus != Shifting.None)
            {
                this.engine.HandleRpmTransitionWhenShifting();
            }
        }

        public void Accelerate()
        {
            this.engine.Accelerate();
        }

        public void Lift()
        {
            this.engine.Lift();
        }

        public void Braking()
        {
            this.engine.Braking();
        }

        public void StateUp(int velocity, int speed)
        {
            this.gearshift.StateUp((int)this.engine.Acceleration.Y, (int)this.engine.Velocity.Y);
        }

        public void StateDown(int velocity, int speed)
        {
            this.gearshift.StateDown((int)this.engine.Acceleration.Y, (int)this.engine.Velocity.Y);
        }

        public void TurnLeft()
        {
            this.steering.TurnLeft();
        }

        public void TurnRight()
        {
            this.steering.TurnRight();
        }

        public void StraightenWheel()
        {
            this.steering.StraightenWheel();
        }
    }


}

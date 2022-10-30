namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Diagnostics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.InputHandler;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;

    public class PowertrainManager : SystemComponent, IPowertrain
    {
        private IPowertrainPacket powertrainPacket;

        private IEngine engine;
        private ISteering steering;
        private IGearshift gearshift;

        public PowertrainManager(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;
            this.gearshift = new Gearshift();
            this.engine = new Engine(this.gearshift);
            this.steering = new Steering();
        }

        public void OnSteering(object sender, ControlEventArgs eventArgs)
        {
            this.steering.Seed(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y, World.Instance.ControlledCar.Rotation);
            switch (eventArgs.Steering)
            {
                case SteeringState.Left:
                    this.steering.CarSpeed = this.engine.Speed;
                    this.steering.TurnLeft();
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    break;
                case SteeringState.Right:
                    this.steering.CarSpeed = this.engine.Speed;
                    this.steering.TurnRight();
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    break;
                case SteeringState.Center:
                    this.steering.CarSpeed = this.engine.Speed;
                    this.steering.StraightenWheel();
                    this.steering.GetRotation();
                    World.Instance.ControlledCar.Rotation = this.steering.Rotation;
                    break;
            }
        }

        public void OnPedal(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Pedal)
            {
                case Pedals.Empty:
                    this.engine.Lift();
                    World.Instance.ControlledCar.Y -= this.engine.Speed;
                    break;
                case Pedals.Brake:
                    this.engine.Breaking();
                    //World.Instance.ControlledCar.Y += 5;
                    break;
                case Pedals.Gas:
                    this.engine.Accelerate();
                    //World.Instance.ControlledCar.Y -= 5;
                    World.Instance.ControlledCar.Y -= this.engine.Speed;
                    break;
            }
        }

        public void OnGearbox(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Gear)
            {
                case Gears.ShiftUp:
                    this.engine.StateUp();
                    this.steering.State += 1;
                    break;
                case Gears.ShiftDown:
                    this.engine.StateDown();
                    this.steering.State -= 1;
                    break;
            }
        }

        public override void Process()
        {
            Debug.WriteLine(this.virtualFunctionBus.InputPacket.ToString());
        }
    }
}

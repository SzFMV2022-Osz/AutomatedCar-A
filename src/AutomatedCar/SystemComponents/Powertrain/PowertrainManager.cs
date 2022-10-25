namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
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
            this.steering = new Steering(0,0,0,0);

            ControlMessenger.SteeringEventHandler += OnSteering;
            ControlMessenger.PedalEventHandler += OnPedal;
            ControlMessenger.GearboxEventHandler += OnGearbox;
        }


        public void OnSteering(object sender, ControlEventArgs eventArgs)
        {
           
        }

        public void OnPedal(object sender, ControlEventArgs eventArgs)
        {
            ;
            switch (eventArgs.Pedal)
            {
                case Pedals.Empty: engine.Lift(); break;
                case Pedals.Brake: engine.Breaking(); break;
               // case Pedals.Gas: engine.; break;
            }
        }

        public void OnGearbox(object sender, ControlEventArgs eventArgs)
        {
            ;
            switch (eventArgs.Gear)
            {
                case Gears.ShiftUp: engine.StateUp(); break;
                case Gears.ShiftDown: engine.StateDown(); break;
            }
        }

        public override void Process()
        {
          
        }
    }
}

namespace AutomatedCar.SystemComponents.InputManager.InputHandler
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Input;

    public class InputManager : SystemComponent
    {
        public InputPacket InputPacket { get; set; }

        public InputManager(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.InputPacket = new InputPacket();
            virtualFunctionBus.InputPacket = this.InputPacket;

            ControlMessenger.Instance.SteeringEventHandler += OnSteering;
            ControlMessenger.Instance.PedalEventHandler += OnPedal;
            ControlMessenger.Instance.GearboxEventHandler += OnGearbox;
        }

        private bool IsGearStateJustChanged(Gears newGearState)
        {
            return InputPacket.GearState != newGearState;
        }

        public void OnSteering(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Steering)
            {
                case SteeringState.Left:
                    this.InputPacket.SteeringState = SteeringState.Left;
                    break;
                case SteeringState.Right:
                    this.InputPacket.SteeringState = SteeringState.Right;
                    break;
                case SteeringState.Center:
                    this.InputPacket.SteeringState = SteeringState.Center;
                    break;
            }
        }
       
        public void OnPedal(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Pedal)
            {
                case Pedals.Gas:
                    this.InputPacket.PedalState = Pedals.Gas;
                    break;
                case Pedals.Empty:
                    this.InputPacket.PedalState = Pedals.Empty;
                    break;
                case Pedals.Brake:
                    this.InputPacket.PedalState = Pedals.Brake;
                    break;
            }
        }

        public void OnGearbox(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Gear)
            {
                case Gears.ShiftUp:
                    this.InputPacket.GearState = Gears.ShiftUp;
                    break;
                case Gears.ShiftDown:
                    this.InputPacket.GearState = Gears.ShiftDown;
                    break;
                case Gears.Steady:
                    this.InputPacket.GearState = Gears.Steady;
                    break;
            }
        }

        
        public override void Process()
        {
            // IsGearStateJustChanged (todo)

        }
    }
}

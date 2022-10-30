namespace AutomatedCar.SystemComponents.InputManager.InputHandler
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia.Input;

    public class InputManager : SystemComponent
    {
        public InputPacket InputPacket { get; set; }

        public InputManager(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            InputPacket = new InputPacket();
            virtualFunctionBus.InputPacket = InputPacket;
        }

        private bool IsGearStateJustChanged(Gears newGearState)
        {
            return InputPacket.GearState != newGearState;
        }

        public override void Process()
        {
            // steering
            if (Keyboard.IsKeyDown(Key.Left))
            {
                InputPacket.SteeringState = SteeringState.Left;
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                InputPacket.SteeringState = SteeringState.Right;
            }
            else
            {
                InputPacket.SteeringState = SteeringState.Center;
            }

            // pedal
            if (Keyboard.IsKeyDown(Key.Up))
            {
                InputPacket.PedalState = Pedals.Gas;
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                InputPacket.PedalState = Pedals.Brake;
            }
            else
            {
                InputPacket.PedalState = Pedals.Empty;
            }

            // gear
            if (Keyboard.IsKeyDown(Key.PageUp))
            {
                InputPacket.IsGearStateJustChanged = IsGearStateJustChanged(Gears.ShiftUp);
                InputPacket.GearState = Gears.ShiftUp;
            }
            else if (Keyboard.IsKeyDown(Key.PageDown))
            {
                InputPacket.IsGearStateJustChanged = IsGearStateJustChanged(Gears.ShiftDown);
                InputPacket.GearState = Gears.ShiftDown;
            }
            else
            {
                InputPacket.GearState = Gears.Steady;
                InputPacket.IsGearStateJustChanged = false;
            }
        }
    }
}

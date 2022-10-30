namespace AutomatedCar.SystemComponents.InputManager
{
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
        }

        private bool IsGearStateJustChanged(Gears newGearState)
        {
            return this.InputPacket.GearState != newGearState;
        }

        public override void Process()
        {
            // steering
            if (Keyboard.IsKeyDown(Key.Left))
            {
                this.InputPacket.SteeringState = SteeringState.Left;
            }
            else if (Keyboard.IsKeyDown(Key.Left))
            {
                this.InputPacket.SteeringState = SteeringState.Right;
            }
            else
            {
                this.InputPacket.SteeringState = SteeringState.Center;
            }

            // pedal
            if (Keyboard.IsKeyDown(Key.Up))
            {
                this.InputPacket.PedalState = Pedals.Gas;
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                this.InputPacket.PedalState = Pedals.Brake;
            }
            else
            {
                this.InputPacket.PedalState = Pedals.Empty;
            }

            // gear
            if (Keyboard.IsKeyDown(Key.PageUp))
            {
                this.InputPacket.IsGearStateJustChanged = this.IsGearStateJustChanged(Gears.ShiftUp);
                this.InputPacket.GearState = Gears.ShiftUp;
            }
            else if (Keyboard.IsKeyDown(Key.PageDown))
            {
                this.InputPacket.IsGearStateJustChanged = this.IsGearStateJustChanged(Gears.ShiftDown);
                this.InputPacket.GearState = Gears.ShiftDown;
            }
            else
            {
                this.InputPacket.GearState = Gears.Steady;
                this.InputPacket.IsGearStateJustChanged = false;
            }
        }
    }
}

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;

    public class InputPacket
    {
        public SteeringState SteeringState { get; set; }

        public Pedals PedalState { get; set; }

        public Gears GearState { get; set; }

        public bool IsGearStateJustChanged { get; set; }

        public InputPacket()
        {
            this.SteeringState = SteeringState.Center;
            this.PedalState = Pedals.Empty;
            this.GearState = Gears.Steady;
            this.IsGearStateJustChanged = false;
        }

        // for logs
        public override string ToString()
        {
            return $"{this.SteeringState.ToString()} - {this.PedalState.ToString()} - {this.GearState.ToString()} - GearStateJustChanged: {this.IsGearStateJustChanged}";
        }
    }
}

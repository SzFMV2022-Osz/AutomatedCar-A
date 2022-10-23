namespace AutomatedCar.SystemComponents.Packets
{
    public interface IPowertrainPacket
    {
        public int Rpm { get; set; }
        public int Kmh { get; set; }
        public int CurrentGear { get; set; }
        public double ThrottleValue { get; set; }
        public double BrakeValue { get; set; }
        public double RotationPoint { get; set; }
        public int Torque { get; set; }
    }
}

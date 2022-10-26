namespace AutomatedCar.SystemComponents.Packets
{
    public interface IPowertrainPacket
    {
        double BrakeValue { get; set; }
        int CurrentGear { get; set; }
        int Kmh { get; set; }
        double RotationPoint { get; set; }
        int Rpm { get; set; }
        double ThrottleValue { get; set; }
        int Torque { get; set; }
    }
}
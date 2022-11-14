namespace AutomatedCar.SystemComponents.Packets
{
    using System;

    public class PowertrainPacket : IPowertrainPacket
    {
        public int Rpm { get; set; }
        public int Kmh { get; set; }
        public int CurrentGear { get; set; }
        public double ThrottleValue { get; set; }
        public double BrakeValue { get; set; }
        public double RotationPoint { get; set; }
        public int Torque { get; set; }
        public PowertrainPacket(int inputRpm)
        {
            Rpm = inputRpm; //example of usage
        }
    }
}

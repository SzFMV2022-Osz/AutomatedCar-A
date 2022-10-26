namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PowertrainPacket : ReactiveObject, IPowertrainPacket
    {
        public int Rpm { get; set; }

        // speed
        public int Kmh { get; set; }
        public int CurrentGear { get; set; }
        public double ThrottleValue { get; set; }
        public double BrakeValue { get; set; }
        public double RotationPoint { get; set; }
        public int Torque { get; set; }
    }
}

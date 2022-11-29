namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CruiseControlPacket : ReactiveObject
    {
        private bool accEnabled;
        private int targetSpeed;
        private double targetDistance;

        public bool ACCEnabled
        {
            get => accEnabled;
            set => this.RaiseAndSetIfChanged(ref this.accEnabled, value);
        }

        public int TargetSpeed
        {
            get => targetSpeed;
            set => this.RaiseAndSetIfChanged(ref this.targetSpeed, value);
        }

        public double TargetDistance
        {
            get => targetDistance;
            set => this.RaiseAndSetIfChanged(ref this.targetDistance, value);
        }
    }
}

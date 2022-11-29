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
        private int targetDistance;

        public bool ACCEnabled
        {
            get => true;
            set => this.RaiseAndSetIfChanged(ref this.accEnabled, value);
        }

        public int TargetSpeed
        {
            get => 0;
            set => this.RaiseAndSetIfChanged(ref this.targetSpeed, value);
        }

        public int TargetDistance
        {
            get => 0;
            set => this.RaiseAndSetIfChanged(ref this.targetDistance, value);
        }
    }
}

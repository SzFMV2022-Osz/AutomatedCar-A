namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AEBPacket : ReactiveObject, IReadOnlyAEBPacket
    {
        private bool collisionPredicted;
        private bool emergencyBreakActivated;
        private bool shouldShowAEBWarning;

        public bool CollisionPredicted
        {
            get => this.collisionPredicted;
            set => this.RaiseAndSetIfChanged(ref this.collisionPredicted, value);
        }


        public bool EmergencyBreakActivated
        {
            get => this.emergencyBreakActivated;
            set => this.RaiseAndSetIfChanged(ref this.emergencyBreakActivated, value);
        }

        public bool ShouldShowAEBWarning
        {
            get => this.shouldShowAEBWarning;
            set => this.RaiseAndSetIfChanged(ref this.shouldShowAEBWarning, value);
        }
    }
}

namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LKAPacket : ReactiveObject, ILKAPacket
    {
        private bool laneKeepingAvailable;
        private bool laneKeepinStatus;

        public bool LaneKeepingAvailable
        {
            get
            {
                return this.laneKeepingAvailable;
            }

            set => this.RaiseAndSetIfChanged(ref this.laneKeepingAvailable, value);
        }

        public bool LaneKeepingStatus
        {
            get
            {
                return this.laneKeepinStatus;
            }

            set => this.RaiseAndSetIfChanged(ref this.laneKeepinStatus, value);
        }
    }
}

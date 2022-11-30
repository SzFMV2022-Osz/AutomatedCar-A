namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LaneKeepingPacket : ReactiveObject, ILaneKeepingPacket
    {
        /// <summary>
        /// When we start the program the default value is LKA turned off.
        /// </summary>
        private LaneKeepingStatus laneKeepingChange = LaneKeepingStatus.Deactivated;

        /// <summary>
        /// Gets or sets the current status.
        /// </summary>
        public LaneKeepingStatus LaneKeepingStatus { get => this.laneKeepingChange; set => this.RaiseAndSetIfChanged(ref this.laneKeepingChange, value); }
    }
}

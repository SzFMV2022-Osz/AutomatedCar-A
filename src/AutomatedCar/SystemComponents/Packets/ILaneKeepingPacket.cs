namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILaneKeepingPacket
    {
        /// <summary>
        /// Gets or sets the lanekeeping status.
        /// It is an 'On' or 'Off' enum value.
        /// </summary>
        public LaneKeepingStatus LaneKeepingStatus { get; set; }
    }
}

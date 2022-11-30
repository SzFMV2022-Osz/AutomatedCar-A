namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRadarPacket : ISensorPacket
    {
        WorldObject ClosestInLane { get; set; }

        WorldObject Closest { get; set; }

        Dictionary<WorldObject, WorldObjectTracker> ObjectTrackingDatas { get; set; }
    }
}

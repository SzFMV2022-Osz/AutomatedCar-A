namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;

    public interface ISensorPacket
    {
        List<WorldObject> RelevantWorldObjs { get; set; }
    }
}

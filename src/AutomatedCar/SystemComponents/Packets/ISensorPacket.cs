namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public interface ISensorPacket
    {
        List<WorldObject> RelevantWorldObjs { get; set; }
    }
}

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using Avalonia.Collections;
    using System.Collections.Generic;

    public interface ISensorPacket
    {
        AvaloniaList<WorldObject> RelevantWorldObjs { get; set; }
    }
}

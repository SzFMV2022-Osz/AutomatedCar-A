namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal interface ISensorPacket
    {
        List<WorldObject> RelevantWorldObjs { get; set; }
    }
}

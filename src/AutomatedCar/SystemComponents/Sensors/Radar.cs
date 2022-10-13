namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class Radar : Sensor
    {
        public Radar(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            throw new NotImplementedException();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            throw new NotImplementedException();
        }
    }
}

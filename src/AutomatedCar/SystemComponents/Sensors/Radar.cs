namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public class Radar : Sensor
    {
        public Radar(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
        }

        public override void Process()
        {
            this.X = new Vector2(this.GetAutomatedCar().X, this.GetAutomatedCar().Y);
            this.Y = new Vector2(this.GetAutomatedCar().X + 200, this.GetAutomatedCar().Y + (230.94f / 2));
            this.Z = new Vector2(this.GetAutomatedCar().X + 200, this.GetAutomatedCar().Y - (230.94f / 2));
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> filtered = this.FilterObjectsInSensor();
            return filtered;
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.sensorPacket = new SensorPacket();
            this.sensorPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }
    }
}

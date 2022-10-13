namespace AutomatedCar.SystemComponents.Sensors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public abstract class Sensor : SystemComponent
    {
        protected float HorizontalDistance { get; set; }

        protected float VerticalDistance { get; set; }

        protected ISensorPacket sensorPacket;

        protected SensorTriangle triangle;

        protected struct SensorTriangle
        {
            public Vector2 X { get; set; }

            public Vector2 Y { get; set; }

            public Vector2 Z { get; set; }
        }

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.triangle = new SensorTriangle();
        }

        protected abstract void SaveWorldObjectsToPacket();

        protected abstract List<WorldObject> FilterRelevantWorldObjects();

        protected List<WorldObject> GetWorldObjects()
        {
            return World.Instance.WorldObjects;
        }
    }
}

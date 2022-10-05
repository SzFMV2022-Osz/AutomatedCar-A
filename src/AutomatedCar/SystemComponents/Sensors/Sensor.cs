namespace AutomatedCar.SystemComponents.Sensors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public abstract class Sensor : SystemComponent
    {
        protected ISensorPacket sensorPacket;

        protected Vector2 X { get; set; }

        protected Vector2 Y { get; set; }

        protected Vector2 Z { get; set; }

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
        }

        protected abstract void SaveWorldObjectsToPacket();

        protected AutomatedCar GetAutomatedCar()
        {
            return World.Instance.ControlledCar;
        }

        protected List<WorldObject> GetWorldObjects()
        {
            return World.Instance.WorldObjects;
        }

        protected List<WorldObject> FilterObjectsInSensor()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            return objs.Where(obj => this.X.X <= obj.X && this.Y.X >= obj.X && this.Z.Y <= obj.Y && this.Y.Y >= obj.Y).ToList();
        }

        protected abstract List<WorldObject> FilterRelevantWorldObjects();
    }
}

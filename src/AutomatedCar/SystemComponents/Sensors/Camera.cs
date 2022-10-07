namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class Camera : Sensor
    {
        public Camera(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.HorizontalDistance = 50 * 80;
            this.VerticalDistance = 50 * (92.37f / 2);
            this.CalculateCoordinates();
        }

        public override void Process()
        {
            this.CalculateCoordinates();
        }

        private void CalculateCoordinates()
        {
            this.triangle.X = new Vector2(this.virtualFunctionBus.CarCoordinatesPacket.X + this.HorizontalDistance, this.virtualFunctionBus.CarCoordinatesPacket.Y);
            this.triangle.Y = new Vector2(this.virtualFunctionBus.CarCoordinatesPacket.X + this.HorizontalDistance, this.virtualFunctionBus.CarCoordinatesPacket.Y + this.VerticalDistance);
            this.triangle.Z = new Vector2(this.virtualFunctionBus.CarCoordinatesPacket.X + this.HorizontalDistance, this.virtualFunctionBus.CarCoordinatesPacket.Y - this.VerticalDistance);
        }

        //Todo: Filter Road, highlight nearest object.
        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            return objs.Where(obj => this.triangle.X.X <= obj.X && this.triangle.Y.X >= obj.X && this.triangle.Z.Y <= obj.Y && this.triangle.Y.Y >= obj.Y).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.sensorPacket = new SensorPacket();
            this.sensorPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }
    }
}

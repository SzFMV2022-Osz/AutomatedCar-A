namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
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
            this.triangle.X = new Point(this.GetAutomatedCar().X + this.HorizontalDistance, this.GetAutomatedCar().Y);
            this.triangle.Y = new Point(this.GetAutomatedCar().X + this.HorizontalDistance, this.GetAutomatedCar().Y + this.VerticalDistance);
            this.triangle.Z = new Point(this.GetAutomatedCar().X + this.HorizontalDistance, this.GetAutomatedCar().Y - this.VerticalDistance);
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            objs = objs.Where(obj => CollisionDetection.PointInTriangle(new Point(obj.X, obj.Y), new Tuple<Point, Point, Point>(this.triangle.X, this.triangle.Y, this.triangle.Z))).ToList();
            return objs.OrderBy(t => t.X).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.sensorPacket = new SensorPacket();
            this.sensorPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }

        protected List<WorldObject> FilterRoad()
        {
            return this.sensorPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road).ToList();
        }

        protected WorldObject NearestWorldObject()
        {
            return this.sensorPacket.RelevantWorldObjs.FirstOrDefault();
        }
    }
}

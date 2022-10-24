namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
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
            this.triangle.X = new Point(this.GetAutomatedCar().X, this.GetAutomatedCar().Y);
            this.triangle.Y = new Point(this.GetAutomatedCar().X + this.HorizontalDistance, this.GetAutomatedCar().Y + this.VerticalDistance);
            this.triangle.Z = new Point(this.GetAutomatedCar().X + this.HorizontalDistance, this.GetAutomatedCar().Y - this.VerticalDistance);
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            return objs.Where(obj => (obj.WorldObjectType == WorldObjectType.Road || obj.WorldObjectType == WorldObjectType.RoadSign) && CollisionDetection.PointInTriangle(new Point(obj.X, obj.Y), new Tuple<Point, Point, Point>(this.triangle.X, this.triangle.Y, this.triangle.Z))).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.virtualFunctionBus.SensorPacket = new SensorPacket();
            this.virtualFunctionBus.SensorPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }

        protected void OrderByDistance()
        {
            //distance = sqrt((x2-x1)^2+(y2-y1)^2)
            this.virtualFunctionBus.SensorPacket.RelevantWorldObjs = this.virtualFunctionBus.SensorPacket.RelevantWorldObjs.OrderBy(obj => Math.Sqrt(Math.Pow((obj.X - this.triangle.X.X), 2) + Math.Pow((obj.Y - this.triangle.X.Y), 2))).ToList();
        }

        protected List<WorldObject> FilterRoad()
        {
            return this.virtualFunctionBus.SensorPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road).ToList();
        }

        protected WorldObject NearestWorldObject()
        {
            return this.virtualFunctionBus.SensorPacket.RelevantWorldObjs.FirstOrDefault();
        }
    }
}

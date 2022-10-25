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
            double deg = 60;
            int dist = 80;

            this.vision = SensorVision.CalculateVision(dist, deg, new Point(this.GetAutomatedCar().X, this.GetAutomatedCar().Y));
        }

        public override void Process()
        {
            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            return objs.Where(obj => (obj.WorldObjectType == WorldObjectType.Road || obj.WorldObjectType == WorldObjectType.RoadSign) && this.IsRelevant(obj)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.virtualFunctionBus.CameraPacket = new SensorPacket();
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }

        private bool IsRelevant(WorldObject obj)
        {
            var objPoly = CollisionDetection.TransformRawGeometry(obj);

            var roi = this.GetROI();

            bool isInTriangle = false;
            for (int i = 0; i < objPoly.Points.Count && !isInTriangle; ++i)
            {
                isInTriangle = CollisionDetection.PointInTriangle(objPoly.Points[i], new Tuple<Point, Point, Point>(roi.Item1, roi.Item2, roi.Item3));
            }

            return isInTriangle;
        }

        protected List<WorldObject> FilterRoad()
        {
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road).ToList();
        }

        protected WorldObject NearestWorldObject()
        {
            OrderByDistance();
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.FirstOrDefault();
        }

        private void OrderByDistance()
        {
            //distance = sqrt((x2-x1)^2+(y2-y1)^2)
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.OrderBy(obj => Math.Sqrt(Math.Pow(obj.X - this.vision.SensorPos.X, 2) + Math.Pow(obj.Y - this.vision.SensorPos.Y, 2))).ToList();
        }
    }
}

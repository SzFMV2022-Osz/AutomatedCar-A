namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    public class Radar : Sensor
    {
        private double deg;
        private int dist;
        private int distInGame;

        private WorldObject radarViewField;
        private PolylineGeometry viewFieldGeometry;

        private WorldObject laneViewField;
        private PolylineGeometry laneFieldGeometry;

        public Radar(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.deg = 60;
            this.dist = 200;
            this.distInGame = 50 * this.dist;

            this.vision = SensorVision.CalculateVision(this.dist, this.deg, new Point(0, 0));
            this.virtualFunctionBus.RadarPacket = new RadarPacket();
            this.virtualFunctionBus.RadarPacket.ObjectTrackingDatas = new Dictionary<WorldObject, WorldObjectTracker>();
        }

        private void InitRadarViewField()
        {
            var car = World.Instance.ControlledCar;
            var carWidth = car.RawGeometries[0].Points.Max(x => x.X);
            var carHeight = car.RawGeometries[0].Points.Max(x => x.Y);

            Point radarPos = new Point(car.RotationPoint.X, car.Geometry.Bounds.Top);

            this.radarViewField = new WorldObject(0, 0, string.Empty);
            var radarViewFieldGeometries = new PolylineGeometry();
            radarViewFieldGeometries.Points.Add(this.vision.SensorPos + radarPos);
            radarViewFieldGeometries.Points.Add(this.vision.Left + radarPos);
            radarViewFieldGeometries.Points.Add(this.vision.Right + radarPos);

            this.radarViewField.RawGeometries.Add(radarViewFieldGeometries);
            this.radarViewField.RotationPoint = car.RotationPoint;

            Point lanePos = new Point(car.Geometry.Bounds.Left, car.Geometry.Bounds.Top);

            this.laneViewField = new WorldObject(0, 0, string.Empty);
            PolylineGeometry laneViewFieldGeometries = new PolylineGeometry();
            Point relativeLanePoint = new Point(carWidth, -this.distInGame);
            laneViewFieldGeometries.Points.Add(new Point(0, 0) + lanePos);
            laneViewFieldGeometries.Points.Add(new Point(relativeLanePoint.X, 0) + lanePos);
            laneViewFieldGeometries.Points.Add(relativeLanePoint + lanePos);
            laneViewFieldGeometries.Points.Add(new Point(0, relativeLanePoint.Y) + lanePos);

            this.laneViewField.RawGeometries.Add(laneViewFieldGeometries);
            this.laneViewField.RotationPoint = car.RotationPoint;
        }

        public override void Process()
        {
            if (this.radarViewField is null)
            {
                this.InitRadarViewField();
            }

            var car = this.GetAutomatedCar();

            this.radarViewField.X = car.X;
            this.radarViewField.Y = car.Y;
            this.radarViewField.Rotation = car.Rotation;
            this.viewFieldGeometry = CollisionDetection.TransformRawGeometry(this.radarViewField);

            this.laneViewField.X = car.X;
            this.laneViewField.Y = car.Y;
            this.laneViewField.Rotation = car.Rotation;
            this.laneFieldGeometry = CollisionDetection.TransformRawGeometry(this.laneViewField);

            this.SaveWorldObjectsToPacket();
        }

        public void UpdateTracking(List<WorldObject> list)
        {
            var objectTrackingDatas = this.virtualFunctionBus.RadarPacket.ObjectTrackingDatas;

            // Out of sight check.
            foreach (var entry in objectTrackingDatas)
            {
                if (!list.Contains(entry.Key) && entry.Key != World.Instance.ControlledCar)
                {
                    objectTrackingDatas.Remove(entry.Key);
                }
            }

            var timestamp = DateTime.Now;

            // Always add controlled car's position
            if (!objectTrackingDatas.ContainsKey(World.Instance.ControlledCar))
            {
                objectTrackingDatas[World.Instance.ControlledCar] = new WorldObjectTracker();
            }

            objectTrackingDatas[World.Instance.ControlledCar].AddPoint(
                new Point(World.Instance.ControlledCar.X, World.Instance.ControlledCar.Y),
                timestamp
                );

            foreach (var obj in list)
            {
                if (!objectTrackingDatas.ContainsKey(obj))
                {
                    objectTrackingDatas[obj] = new WorldObjectTracker();
                }

                objectTrackingDatas[obj].AddPoint(new Point(obj.X, obj.Y), timestamp);
            }
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            return this.GetWorldObjects().Where(x => this.IsRelevant(x)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            var list = this.FilterRelevantWorldObjects();
            list = this.OrderByDistance(list);

            this.UpdateTracking(list);

            this.virtualFunctionBus.RadarPacket.RelevantWorldObjs = list;
            this.virtualFunctionBus.RadarPacket.Closest = this.NearestWorldObject(list);
            this.virtualFunctionBus.RadarPacket.ClosestInLane = this.ClosestInLane(list);
        }

        private bool IsRelevant(WorldObject obj)
        {
            var radarTriangle = new Tuple<Point, Point, Point>(
                this.viewFieldGeometry.Points[0],
                this.viewFieldGeometry.Points[1],
                this.viewFieldGeometry.Points[2]);

            var car = this.GetAutomatedCar();

            if (obj.Equals(car))
            {
                return false;
            }

            if (obj.WorldObjectType != WorldObjectType.Car
                && obj.WorldObjectType != WorldObjectType.RoadSign
                && obj.WorldObjectType != WorldObjectType.Tree
                && obj.WorldObjectType != WorldObjectType.Pedestrian
                && obj.WorldObjectType != WorldObjectType.Other)
            {
                return false;
            }

            var objPoly = CollisionDetection.TransformRawGeometry(obj);

            bool isInTriangle = false;
            for (int i = 0; i < objPoly.Points.Count && !isInTriangle; ++i)
            {
                isInTriangle = CollisionDetection.PointInTriangle(objPoly.Points[i], radarTriangle);
            }

            return isInTriangle;
        }

        private List<WorldObject> OrderByDistance(List<WorldObject> list)
        {
            // distance = sqrt((x2-x1)^2+(y2-y1)^2)
            return list.OrderBy(obj => Math.Sqrt(Math.Pow(obj.X - this.vision.SensorPos.X, 2) + Math.Pow(obj.Y - this.vision.SensorPos.Y, 2))).ToList();
        }

        private WorldObject ClosestInLane(List<WorldObject> relevantList)
        {
            if (relevantList.Count == 0)
            {
                return null;
            }

            double closestDist = double.MaxValue;
            WorldObject closest = null;

            for (int i = 0; i < relevantList.Count; ++i)
            {
                var transformed = CollisionDetection.TransformRawGeometry(relevantList[i]);
                bool collide = CollisionDetection.BoundingBoxesCollide(this.laneFieldGeometry, transformed, 1);

                if (!collide)
                {
                    continue;
                }

                double calculated = Math.Sqrt(Math.Pow(this.vision.SensorPos.X - relevantList[i].X, 2) + Math.Pow(this.vision.SensorPos.Y - relevantList[i].Y, 2));
                if (calculated < closestDist)
                {
                    closestDist = calculated;
                    closest = relevantList[i];
                }
            }

            return closest;
        }

        private WorldObject NearestWorldObject(List<WorldObject> list)
        {
            if (list.Count == 0)
            {
                return null;
            }

            return list[0];
        }
    }
}

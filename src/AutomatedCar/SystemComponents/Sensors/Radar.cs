namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.Models.NPC;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    public class Radar : Sensor
    {
        private double deg;
        private int dist;
        private int distInGame;

        public Radar(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.deg = 60;
            this.dist = 200;
            this.distInGame = 50 * this.dist;

            // finding car's width
            //var carWith = this.GetAutomatedCar().RawGeometries[0].Points.Max(x => x.X);

            // positioning sensor on the car
            this.vision = SensorVision.CalculateVision(this.dist, this.deg, new Point(0, 0));
            this.virtualFunctionBus.RadarPacket = new RadarPacket();

        }

        public override void Process()
        {
            if (World.Instance.controlledCars.Count == 0)
            {
                return;
            }

            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            var list = this.GetWorldObjects().Where(x => this.IsRelevant(x)).ToList();
            return list;

        }

        protected override void SaveWorldObjectsToPacket()
        {
            var list = this.FilterRelevantWorldObjects();
            list = this.OrderByDistance(list);

            this.virtualFunctionBus.RadarPacket.RelevantWorldObjs = list;

            this.virtualFunctionBus.RadarPacket.Closest = this.NearestWorldObject(list);
            this.virtualFunctionBus.RadarPacket.ClosestInLane = this.ClosestInLane(list);
            /*
            Trace.WriteLine(packet.RelevantWorldObjs.Count);
            packet.RelevantWorldObjs.ForEach(x => Trace.Write(x.X + "," + x.Y + " " + x.Filename + "; "));
            Trace.WriteLine(" ");

            Trace.WriteLine("CLOSEST: ");
            Trace.WriteLine(packet.Closest == null ? "no closest" : packet.Closest.Filename + "," + packet.Closest.X + ", " + packet.Closest.Y);
            Trace.WriteLine("CLOSEST IN LANE:");
            Trace.WriteLine(packet.ClosestInLane == null ? "no closest" : packet.ClosestInLane.Filename + ", " + packet.ClosestInLane.X + ", " + packet.ClosestInLane.Y);
            */

        }

        private bool IsRelevant(WorldObject obj)
        {
            AutomatedCar car = this.GetAutomatedCar();

            if (obj.Equals(car))
            {
                return false;
            }

            if (!(obj is INPC) && !(obj is Car))
            {
                return false;
            }

            var objPoly = CollisionDetection.TransformRawGeometry(obj);

            var roi = this.GetROI();

            bool isInTriangle = false;
            for (int i = 0; i < objPoly.Points.Count && !isInTriangle; ++i)
            {
                isInTriangle = CollisionDetection.PointInTriangle(objPoly.Points[i], new Tuple<Point, Point, Point>(roi.Item1, roi.Item2, roi.Item3));
            }

            return isInTriangle;
        }

        private List<WorldObject> OrderByDistance(List<WorldObject> list)
        {
            var sensorPos = this.GetROI().Item3;

            // distance = sqrt((x2-x1)^2+(y2-y1)^2)
            return list.OrderBy(obj => Math.Sqrt(Math.Pow(obj.X - sensorPos.X, 2) + Math.Pow(obj.Y - sensorPos.Y, 2))).ToList();
        }

        private WorldObject ClosestInLane(List<WorldObject> relevantList)
        {
            if (relevantList.Count == 0)
            {
                return null;
            }

            var car = this.GetAutomatedCar();
            var carPos = new Point(car.X, car.Y);
            var roi = this.GetROI();

            var carWidth = this.GetAutomatedCar().RawGeometries[0].Points.Max(x => x.X);

            Point relativePoint = new Point(carWidth, -this.distInGame);
            Point relativeNextPoint = CollisionDetection.RotatePoint(relativePoint, car.Rotation);

            Rect rect = new Rect(car.X, car.Y, relativeNextPoint.X, relativeNextPoint.Y);

            PolylineGeometry poly = new PolylineGeometry();
            poly.Points.Add(rect.TopLeft);
            poly.Points.Add(rect.TopRight);
            poly.Points.Add(rect.BottomLeft);
            poly.Points.Add(rect.BottomRight);

            double closestDist = double.MaxValue;
            WorldObject closest = relevantList[0];
            for (int i = 0; i < relevantList.Count; ++i)
            {
                if (!CollisionDetection.BoundingBoxesCollide(poly, CollisionDetection.TransformRawGeometry(closest), 1))
                {
                    continue;
                }

                double calculated = Math.Sqrt(Math.Pow(roi.Item3.X - relevantList[i].X, 2) + Math.Pow(roi.Item3.Y - relevantList[i].Y, 2));
                if (calculated < closestDist)
                {
                    closestDist = calculated;
                    closest = relevantList[i];
                }
            }

            if (closestDist == double.MaxValue)
            {
                return null;
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

﻿namespace AutomatedCar.SystemComponents.Sensors
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
        public Radar(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            double deg = 60;
            int dist = 200;

            this.vision = SensorVision.CalculateVision(dist, deg, new Point(0, 0));
            virtualFunctionBus.RadarPacket = new SensorPacket();
        }

        public override void Process()
        {
            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            return this.GetWorldObjects().Where(x => this.IsRelevant(x)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            var filtered = this.FilterRelevantWorldObjects();

            /*
            Trace.WriteLine(filtered.Count);
            filtered.ForEach(x => Trace.Write(x.X + "," + x.Y + " " + x.Filename + "; "));
            Trace.WriteLine(" ");
            */
            this.virtualFunctionBus.RadarPacket.RelevantWorldObjs = filtered;
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
    }
}

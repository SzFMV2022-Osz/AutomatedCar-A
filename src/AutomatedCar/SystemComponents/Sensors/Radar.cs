namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public class Radar : Sensor
    {
        private Vector2 visionRightPos;
        private Vector2 visionLeftPos;

        private Vector2 RotatedVisionRightPos
        {
            get
            {
                return this.RotatePosition(this.visionRightPos, this.GetAutomatedCar().Rotation);
            }
        }

        private Vector2 RotatedVisionLeftPos
        {
            get
            {
                return this.RotatePosition(this.visionLeftPos, this.GetAutomatedCar().Rotation);
            }
        }

        public Radar(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            double deg = 60;
            int dist = 200;

            double rad = this.AngleDegToRad(deg);
            int gameDist = 50 * dist;

            int x = (int)(gameDist * Math.Cos(rad));
            int y = (int)(gameDist * Math.Sin(rad));

            this.visionLeftPos = new Vector2(-x, -y);
            this.visionRightPos = new Vector2(x, -y);
        }

        public override void Process()
        {
            var filtered = this.FilterRelevantWorldObjects();
            this.SaveWorldObjectsToPacket(this.FilterRelevantWorldObjects());
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            return this.GetWorldObjects().Where(x => this.IsRelevant(x)).ToList();
        }

        protected override void SaveWorldObjectsToPacket(List<WorldObject> worldObjects)
        {
            ISensorPacket packet = new SensorPacket();
            packet.RelevantWorldObjs = worldObjects;
            this.virtualFunctionBus.RadarPacket = packet;
        }

        private bool IsRelevant(WorldObject obj)
        {
            AutomatedCar car = this.GetAutomatedCar();

            if (obj is AutomatedCar)
            {
                return false;
            }

            var carPos = new Vector2(car.X, car.Y);

            bool isInTriangle = PointInTriangle(
                new Vector2(obj.X, obj.Y),
                carPos,
                carPos + this.RotatedVisionLeftPos,
                carPos + this.RotatedVisionRightPos);

            return obj is Car && isInTriangle;
        }

        /// <summaReturnsry>
        /// Returns whether point is in a triangle with Barycentric method.
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="p0">Triangle Point 1.</param>
        /// <param name="p1">Triangle Point 2.</param>
        /// <param name="p2">Triangle Point 3.</param>
        /// <returns>Point is in triangle or not.</returns>
        public static bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            var s = ((p0.X - p2.X) * (p.Y - p2.Y)) - ((p0.Y - p2.Y) * (p.X - p2.X));
            var t = ((p1.X - p0.X) * (p.Y - p0.Y)) - ((p1.Y - p0.Y) * (p.X - p0.X));

            if ((s < 0) != (t < 0) && s != 0 && t != 0)
        {
                return false;
            }

            var d = ((p2.X - p1.X) * (p.Y - p1.Y)) - ((p2.Y - p1.Y) * (p.X - p1.X));
            return d == 0 || (d < 0) == (s + t <= 0);
        }
    }
}

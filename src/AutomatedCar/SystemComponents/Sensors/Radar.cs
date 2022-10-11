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
            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            return this.GetWorldObjects().Where(x => this.IsRelevant(x)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            ISensorPacket packet = new SensorPacket();
            packet.RelevantWorldObjs = this.FilterRelevantWorldObjects();
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

            bool isInTriangle = this.PointInTriangle(
                new Vector2(obj.X, obj.Y),
                carPos,
                carPos + this.RotatedVisionLeftPos,
                carPos + this.RotatedVisionRightPos);

            return obj is Car && isInTriangle;
        }
    }
}

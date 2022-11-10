namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;

    public abstract class Sensor : SystemComponent
    {
        protected SensorVision vision;

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
        }

        protected abstract List<WorldObject> FilterRelevantWorldObjects();

        protected abstract void SaveWorldObjectsToPacket();

        protected AutomatedCar GetAutomatedCar()
        {
            return World.Instance.ControlledCar;
        }

        protected List<WorldObject> GetWorldObjects()
        {
            return World.Instance.WorldObjects;
        }

        /// <summary>
        /// Calculates absolute coordinates of the sensor's vision.
        /// Item1: Left Pos
        /// Item2: Right Pos
        /// Item3: Sensor Pos.
        /// </summary>
        /// <returns>Region of interest.</returns>
        protected Tuple<Point, Point, Point> GetROI()
        {
            var car = this.GetAutomatedCar();
            var carPos = new Point(car.X, car.Y);
            var sensorOffset = new Point(0, -car.RotationPoint.Y);

            var points = new List<Point>();
            points.Add(CollisionDetection.RotatePoint(this.vision.Left, car.Rotation) + carPos + sensorOffset);
            points.Add(CollisionDetection.RotatePoint(this.vision.Right, car.Rotation) + carPos + sensorOffset);
            points.Add(CollisionDetection.RotatePoint(this.vision.SensorPos, car.Rotation) + carPos + sensorOffset);

            return new Tuple<Point, Point, Point>(points[0], points[1], points[2]);
        }
    }
}

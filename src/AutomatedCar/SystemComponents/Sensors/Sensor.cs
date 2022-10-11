namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    public abstract class Sensor : SystemComponent
    {
        protected float HorizontalDistance { get; set; }

        protected float VerticalDistance { get; set; }

        protected ISensorPacket sensorPacket;

        protected SensorTriangle triangle;

        protected struct SensorTriangle
        {
            public Vector2 X { get; set; }

            public Vector2 Y { get; set; }

            public Vector2 Z { get; set; }
        }

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.triangle = new SensorTriangle();
        }

        protected abstract List<WorldObject> FilterRelevantWorldObjects();

//        protected abstract void SaveWorldObjectsToPacket(List<WorldObject> worldObjects);
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
        /// Converts degree to radian.
        /// </summary>
        /// <param name="angleDeg">Angle in degree.</param>
        /// <returns>Angle in radian.</returns>
        protected double AngleDegToRad(double angleDeg)
        {
            return Math.PI * angleDeg / 180;
        }

        /// <summary>
        /// Rotates a 2D coordinate with angle given in degree around (0,0).
        /// </summary>
        /// <param name="pos">Position in 2D.</param>
        /// <param name="angleDeg">Angle given in degree.</param>
        /// <returns>Rotated coordinate.</returns>
        protected Vector2 RotatePosition(Vector2 pos, double angleDeg)
        {
            double angleRad = this.AngleDegToRad(angleDeg);
            return new Vector2(
                (int)((pos.X * Math.Cos(angleRad)) - (pos.Y * Math.Sin(angleRad))),
                (int)((pos.X * Math.Sin(angleRad)) + (pos.Y * Math.Cos(angleRad))));
        }

        /// <summary>
        /// Returns whether point is in a triangle with Barycentric method.
        /// </summary>
        /// <param name="p">Point</param>
        /// <param name="p0">Triangle Point 1.</param>
        /// <param name="p1">Triangle Point 2.</param>
        /// <param name="p2">Triangle Point 3.</param>
        /// <returns>Point is in triangle or not.</returns>
        protected bool PointInTriangle(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2)
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

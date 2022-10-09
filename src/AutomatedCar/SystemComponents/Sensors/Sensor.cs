namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    internal abstract class Sensor : SystemComponent
    {
        protected ISensorPacket sensorPacket;

        public Sensor(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
        }

        protected abstract List<WorldObject> FilterRelevantWorldObjects();

        protected abstract void SaveWorldObjectsToPacket(List<WorldObject> worldObjects);

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
    }
}

namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using Avalonia;
    using Avalonia.Data;
    using Avalonia.Media;

    public class AutomaticEmergencyBreak : SystemComponent
    {
        public AutomaticEmergencyBreak(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {

        }

        public override void Process()
        {
            // Simulate will be used like this:
            var car = World.Instance.ControlledCar;
            var result = this.Simulate(car, this.virtualFunctionBus.RadarPacket.RelevantWorldObjs);

            if (!double.IsInfinity(result.Item1))
            {
                // Then issue #105 gets called
            }
        }

        /// <summary>
        /// Simulates the next N time step and checks for collision.
        /// </summary>
        /// <returns>Time to collision in seconds, and the car's velocity in meter/sec.</returns>
        private Tuple<double, double> Simulate(WorldObject automatedCar, List<WorldObject> others, int N = 13)
        {
            var tracker = this.virtualFunctionBus.RadarPacket.ObjectTrackingDatas;
            var carTracker = tracker[automatedCar];
            var otherTrackers = others.Select(obj => tracker[obj]).Where(t => t.Points.Count > 1);

            // Exit if we don't have enough data to predict
            if (carTracker.Points.Count < 2 || otherTrackers.Count() == 0)
            {
                return new Tuple<double, double>(double.PositiveInfinity, 0);
            }

            Point carVelocity = this.CalculateVelocityFromTracker(carTracker);
            var otherVelocities = otherTrackers.Select(t => this.CalculateVelocityFromTracker(t));

            var carGeometry = Helpers.CollisionDetection.TransformRawGeometry(automatedCar);
            var otherGeometries = others.Select(obj => Helpers.CollisionDetection.TransformRawGeometry(obj));

            // Translate each geom by it's velocity for N time steps
            for (int i = 0; i < N; i++)
            {
                carGeometry = Helpers.CollisionDetection.TranslateGeometry(carGeometry, carVelocity.X, carVelocity.Y);
                otherGeometries = otherGeometries.Zip(otherVelocities, (geom, velocity) =>
                Helpers.CollisionDetection.TranslateGeometry(geom, velocity.X, velocity.Y));

                if (otherGeometries.Any(obj => Helpers.CollisionDetection.BoundingBoxesCollide(carGeometry, obj, 1)))
                {
                    return new Tuple<double, double>(i * Constants.SecondsBetweenTrack, this.ConvertToMeterPerSec(carVelocity));
                }
            }

            return new Tuple<double, double>(double.PositiveInfinity, 0);
        }

        private Point CalculateVelocityFromTracker(WorldObjectTracker tracker)
        {
            return tracker.Points.ElementAt(1).Item1 - tracker.Points.ElementAt(0).Item1;
        }

        private double ConvertToMeterPerSec(Point velocity)
        {
            return Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) / Constants.SecondsBetweenTrack * Constants.MeterInPixels;
        }

        /// <summary>
        /// Calculates the possible crash point for two objects.
        /// </summary>
        /// <param name="carGeom">Cars's polylineGeometry.</param>
        /// <param name="objGeom">target object's polylineGeometry.</param>
        /// <param name="carPosition">Car's position.</param>
        /// <param name="objPosition">Target's position.</param>
        /// <returns>Returns the closest point where the car could crash with something, returns default Optional otherwise.</returns>
        private Optional<Point> CalculateCollision(PolylineGeometry carGeom, PolylineGeometry objGeom, Point carPosition, Point objPosition)
        {
            PolylineGeometry closest;

            bool couldCrash = CollisionDetection.BoundingBoxesCollide(carGeom, objGeom, 0);
            if (couldCrash)
            {
                closest = objGeom;
            }
            else
            {
                return default(Optional<Point>);
            }

            Point closestPoint = closest.Points[0];
            var closestPointDistance = Math.Sqrt(Math.Pow(carPosition.X - objPosition.X, 2) + Math.Pow(carPosition.Y - objPosition.Y, 2));
            var closestPoints = closest.Points.ToList();
            foreach (var polyPoint in closestPoints)
            {
                var currentPointDistance = Math.Sqrt(Math.Pow(carPosition.X - polyPoint.X, 2) + Math.Pow(carPosition.Y - polyPoint.Y, 2));
                if (carGeom.FillContains(polyPoint) && currentPointDistance < closestPointDistance)
                {
                    closestPoint = polyPoint;
                }
            }

            return new Optional<Point>(closestPoint);
        }
    }
}

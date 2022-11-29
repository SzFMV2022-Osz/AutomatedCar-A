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
        /// Calculates the possible point where the controlled car would crash with something.
        /// </summary>
        /// <param name="objects">Relevant static objects.</param>
        /// <param name="car">Controlled car.</param>
        /// <returns>Returns the closest point where the car could crash with something, returns default Optional otherwise.</returns>
        public Optional<Point> CalculateStaticCollision(List<WorldObject> objects, AutomatedCar car)
        {
            if (objects == null || objects.Count == 0 || car == null)
            {
                return default(Optional<Point>);
            }

            Rect rect = this.virtualFunctionBus.RadarPacket.FrontalRadarArea;
            WorldObject closest = null;
            var closestObjectDistance = double.MaxValue;
            PolylineGeometry poly = new PolylineGeometry();
            poly.Points.Add(rect.TopLeft);
            poly.Points.Add(rect.TopRight);
            poly.Points.Add(rect.BottomLeft);
            poly.Points.Add(rect.BottomRight);
            foreach (var staticObject in objects)
            {
                bool couldCrash = CollisionDetection.BoundingBoxesCollide(poly, CollisionDetection.TransformRawGeometry(staticObject), 0);
                var currentDistance = Math.Sqrt(Math.Pow(car.X - staticObject.X, 2) + Math.Pow(car.Y - staticObject.Y, 2));
                if (couldCrash && currentDistance < closestObjectDistance)
                {
                    closest = staticObject;
                    closestObjectDistance = currentDistance;
                }
            }

            if (closest == null)
            {
                return default(Optional<Point>);
            }

            // Default point where the crash would happen at an exact moment is the central point of the object.
            Point closestPoint = CollisionDetection.TransformRawGeometry(closest).Points[0];
            var closestPointDistance = Math.Sqrt(Math.Pow(car.X - closest.X, 2) + Math.Pow(car.Y - closest.Y, 2));

            // Searching for closer points based on object's polygon points.
            var objectPoints = CollisionDetection.TransformRawGeometry(closest).Points.ToList();
            objectPoints.ForEach(x =>
            {
                var currentPointDistance = Math.Sqrt(Math.Pow(car.X - x.X, 2) + Math.Pow(car.Y - x.Y, 2));
                if (currentPointDistance < closestPointDistance && rect.Contains(x))
                {
                    closestPoint = x;
                    closestPointDistance = Math.Sqrt(Math.Pow(car.X - closestPoint.X, 2) + Math.Pow(car.Y - closestPoint.Y, 2));
                }
            });

            return new Optional<Point>(closestPoint);
        }
    }
}

namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.ConstrainedExecution;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using AutomatedCar.SystemComponents.Sensors;
    using Avalonia;
    using Avalonia.Data;
    using Avalonia.Media;

    public class AutomaticEmergencyBreak : SystemComponent
    {
        private IDinamicObjectPlaceCalculator calculator;
        private AEBPacket packet;

        private Point carVelocity;

        public AutomaticEmergencyBreak(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.calculator = new DinamicObjectPlaceCalculator();
            this.packet = new AEBPacket();
            virtualFunctionBus.AEBPacket = packet;
        }

        public override void Process()
        {
            UseDummySimulate();
        }

        private void UseAdvancedSimulate()
        {
            var car = World.Instance.ControlledCar;
            var obj = this.virtualFunctionBus.RadarPacket.Closest;


            (bool collisionPredicted, Point collisionPoint) = this.AdvancedSimulate(car, obj, 15);

            if (collisionPredicted)
            {
                this.packet.EmergencyBreakActivated = this.AEB(collisionPoint, this.carVelocity);
                // itt kéne jelezni a hajtásláncnak a vészféket valahogy
            }
            else
            {
                this.packet.EmergencyBreakActivated = false;
            }
            this.packet.CollisionPredicted = collisionPredicted;
        }

        private void UseDummySimulate()
        {
            var car = World.Instance.ControlledCar;
            var others = this.virtualFunctionBus.RadarPacket.RelevantWorldObjs;
            Tuple<double, double> result = this.Simulate(car, others);
            double timeToCollision = result.Item1;
            double currentVelocity = result.Item2;

            this.packet.CollisionPredicted = !double.IsInfinity(timeToCollision);
            this.packet.EmergencyBreakActivated = this.AEBActivation(timeToCollision, currentVelocity);

            if (this.packet.EmergencyBreakActivated)
            {
                ControlMessenger.Instance.FireCruiseControlEvent(CruiseControlInputs.TurnOff);
                // Call Powertrain's EmergencyBrake
            }
        }

        // WORK IN PROGRESS
        private (bool, Point) AdvancedSimulate(WorldObject car, WorldObject obj, int N)
        {
            // Exit if car or obj does not exist
            if (obj == null || car == null)
            {
                return (false, default(Point));
            }

            var tracker = this.virtualFunctionBus.RadarPacket.ObjectTrackingDatas;
            var carTracker = tracker[car];
            var objTracker = tracker[obj];

            // Exit if we don't have enough data to predict
            if (carTracker.Points.Count < 3 || objTracker.Points.Count < 3)
            {
                return (false, default(Point));
            }

            var carPath = this.calculator.CalculatePoints(
                carTracker.Points.Dequeue().Item1,
                carTracker.Points.Dequeue().Item1,
                carTracker.Points.Dequeue().Item1,
                N);

            var objPath = this.calculator.CalculatePoints(
                objTracker.Points.Dequeue().Item1,
                objTracker.Points.Dequeue().Item1,
                objTracker.Points.Dequeue().Item1,
                N);

            var carGeom = car.RawGeometries.First();
            var objGeom = obj.RawGeometries.First();

            Point carPosition = new Point(car.X, car.Y);
            Point objPosition = new Point(obj.X, obj.Y);

            double carRotation = car.Rotation;
            double objRotation = obj.Rotation;

            this.carVelocity = carPath.First().Position - carPosition;

            foreach (var futureState in carPath.Zip(objPath))
            {
                var carState = futureState.First;
                var objState = futureState.Second;

                Point carPositionChange = carState.Position - carPosition;
                Point objPositionChange = objState.Position - objPosition;

                double carRotationChange = carState.Rotation - carRotation;
                double objRotationChange = objState.Rotation - objRotation;

                carGeom = Helpers.CollisionDetection.TranslateGeometry(carGeom, carPositionChange.X, carPositionChange.Y);
                objGeom = Helpers.CollisionDetection.TranslateGeometry(objGeom, objPositionChange.X, objPositionChange.Y);

                carGeom = Helpers.CollisionDetection.RotateBoundingBox(carGeom, objRotationChange);
                objGeom = Helpers.CollisionDetection.RotateBoundingBox(objGeom, objRotationChange);

                Optional<Point> collisionPoint = CalculateCollision(carGeom, objGeom, carPosition, objPosition);

                if (collisionPoint.HasValue)
                {
                    return (true, collisionPoint.Value);
                }
            }

            return (false, default(Point));
        }

        /// <summary>
        /// Simulates the next N time step and checks for collision.
        /// </summary>
        /// <returns>Time to collision in seconds, and the car's velocity in meter/sec.</returns>
        private Tuple<double, double> Simulate(WorldObject automatedCar, List<WorldObject> others, int N = 20)
        {
            var tracker = this.virtualFunctionBus.RadarPacket.ObjectTrackingDatas;
            var carTracker = tracker[automatedCar];
            var otherTrackers = others.Select(obj => tracker[obj]).Where(t => t.Points.Count > 1);

            // Exit if we don't have enough data to predict
            if (carTracker.Points.Count < 2 || otherTrackers.Count() == 0)
            {
                return new Tuple<double, double>(double.PositiveInfinity, 0);
            }


            Point carVelocity = carTracker.CalculateAverageSpeedVector(); //this.CalculateVelocityFromTracker(carTracker);
            var otherVelocities = otherTrackers.Select(t => t.CalculateAverageSpeedVector());

            var carGeometry = Helpers.CollisionDetection.TransformRawGeometry(automatedCar);
            var otherGeometries = others.Select(obj => Helpers.CollisionDetection.TransformRawGeometry(obj));

            File.AppendAllText("logx.txt",
                carVelocity.Y.ToString());

            // Translate each geom by it's velocity for N time steps
            for (int i = 0; i < N; i++)
            {
                carGeometry = Helpers.CollisionDetection.TranslateGeometry(carGeometry, carVelocity.X, carVelocity.Y);
                otherGeometries = otherGeometries.Zip(otherVelocities, (geom, velocity) =>
                Helpers.CollisionDetection.TranslateGeometry(geom, velocity.X, velocity.Y));

                if (otherGeometries.Any(obj => CheckForCollision(carGeometry, obj)))
                {
                    File.AppendAllText("logx.txt", "*\n");
                    return new Tuple<double, double>(i * Constants.SecondsBetweenTrack, this.ConvertToMeterPerSec(carVelocity));
                }
            }

            File.AppendAllText("logx.txt", "\n");
            return new Tuple<double, double>(double.PositiveInfinity, 0);
        }

        private Point CalculateVelocityFromTracker(WorldObjectTracker tracker)
        {
            return tracker.Points.ElementAt(1).Item1 - tracker.Points.ElementAt(0).Item1;
        }

        private double ConvertToMeterPerSec(Point velocity)
        {
            return Math.Sqrt(Math.Pow(velocity.X, 2) + Math.Pow(velocity.Y, 2)) / Constants.SecondsBetweenTrack / Constants.MeterInPixels;
        }

        private bool CheckForCollision(PolylineGeometry obj1, PolylineGeometry obj2)
        {
            if (Helpers.CollisionDetection.BoundingBoxesCollide(obj1, obj2, 1))
            {
                return true;
            }
            else
            {
                return Helpers.CollisionDetection.PointInRectangle(obj1.Bounds.Center, obj2.Bounds) ||
                    Helpers.CollisionDetection.PointInRectangle(obj2.Bounds.Center, obj1.Bounds);
            }
        }

        public bool AEB(Point collisionPoint, Point velocity)
        {
            AutomatedCar car = World.Instance.ControlledCar;

            double distanceToCollision = Math.Sqrt(Math.Pow(collisionPoint.X - car.X, 2) + Math.Pow(collisionPoint.Y - car.Y, 2)) / Constants.MeterInPixels;
            double breakingDistance = this.BreakingDistanceCalculator(velocity);
            return breakingDistance >= distanceToCollision;
        }

        private double BreakingDistanceCalculator(Point velocity)
        {
            return Math.Pow(this.ConvertToMeterPerSec(velocity), 2) / (2 * 0.6 * 9.8);
        }

        private double BreakingDistanceCalculator2(Point velocity)
        {
            return (0 - Math.Pow(this.ConvertToMeterPerSec(velocity), 2)) / (2 * (-9));
        }

        private bool AEBActivation(double timeToCollision, double velocity, double maxDeceleration = 9.0)
        {
            if (double.IsInfinity(timeToCollision))
            {
                return false;
            }

            double brakingTime = velocity / maxDeceleration;
            return timeToCollision + Helpers.Constants.BackupTime <= brakingTime;
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

            Point closestPoint = objPosition;
            var closestPointDistance = Math.Sqrt(Math.Pow(carPosition.X - objPosition.X, 2) + Math.Pow(carPosition.Y - objPosition.Y, 2));
            var closestPoints = closest.Points.ToList();
            foreach (var polyPoint in closestPoints)
            {
                var currentPointDistance = Math.Sqrt(Math.Pow(carPosition.X - polyPoint.X, 2) + Math.Pow(carPosition.Y - polyPoint.Y, 2));
                if (carGeom.FillContains(polyPoint) && currentPointDistance < closestPointDistance)
                {
                    closestPoint = polyPoint;
                    closestPointDistance = currentPointDistance;
                }
            }

            return new Optional<Point>(closestPoint);
        }
    }
}

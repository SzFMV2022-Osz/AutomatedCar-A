namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using Newtonsoft.Json;

    public class LaneKeepingAutomatic : SystemComponent
    {
        private List<WorldObject> roads;
        private WorldObject world;
        private AutomatedCar car;
        private LaneKeepingPacket laneKeepingPacket;
        private List<Point> roadPoints;
        // private LKAValidation validation;

        private int x = 2;    // I use this number to 'calibrate' at which point the steer wheel needs to be used.
        private int currentRPM;

        private static IList<worldObject> RefPoints = LoadJson();

        public LaneKeepingAutomatic(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.laneKeepingPacket = new LaneKeepingPacket();
            virtualFunctionBus.LeaneKeepingPacket = this.laneKeepingPacket;
            this.currentRPM = virtualFunctionBus.PowertrainPacket.Rpm;
        }

        public override void Process()
        {
            var car = World.Instance.ControlledCar;
            this.roads = this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.Where(obj => (obj.WorldObjectType == WorldObjectType.Road)).ToList();

            // This packet will be implemented in other way ...
            if (this.laneKeepingPacket.LaneKeepingStatus == LaneKeepingStatus.Activated)
            {
                //foreach (WorldObject wobject in this.roads)
                //{
                    //if (validation.CanBeTurnedOn(GetPoints(wobject)), car.X, car.Y)
                    //{
                        this.Corrigate();
                    //}
                //}
            }
        }

        /// <summary>
        /// Corrigate the car positon from the side of the lane toward to the middle.
        /// Bouncing between the 2 lane line.
        /// </summary>
        public void Corrigate()
        {
            WorldObject closestObject = FindClosestObject(this.roads, this.car);

            // The steering intervention depends on the car orientation and the closest object orientation.
            if (this.car.Rotation > closestObject.Rotation + this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Left;
            }
            else if (this.car.Rotation < closestObject.Rotation - this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Right;
            }

            // this.virtualFunctionBus.InputPacket.PedalState = Pedals.Gas;
            // It needs more intervention support.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the car frontend road coordinates x and y.
        /// </summary>
        /// <param name="worldObject">Roads WorldObject.</param>
        /// <returns>List of road points x and y pair.</returns>
        public static List<Point> GetPoints(WorldObject worldObject)
        {
            Point refPoint = new Point(0, 0);
            if (RefPoints.Any(r => r.Type + ".png" == worldObject.Filename))
            {
                worldObject currRefPoint = RefPoints.Where(r => r.Type + ".png" == worldObject.Filename).FirstOrDefault();
                refPoint = new (currRefPoint.X, currRefPoint.Y);
            }

            List<Point> roadPoints = new () { new Point(worldObject.X, worldObject.Y) };
            foreach (PolylineGeometry currentGeo in worldObject.Geometries)
            {
                foreach (Point currGeoPoint in currentGeo.Points)
                {
                    roadPoints.Add(new Point((worldObject.X + currGeoPoint.X) - refPoint.X, (worldObject.Y + currGeoPoint.Y) - refPoint.Y));
                }
            }

            return roadPoints;
        }

        /// <summary>
        /// Find the closest roadside from the car point.
        /// </summary>
        /// <param name="worldObjects">Roads.</param>
        /// <param name="car">Car.</param>
        /// <returns>Closest object WorldObject.</returns>
        public static WorldObject FindClosestObject(List<WorldObject> worldObjects, AutomatedCar car)
        {
            WorldObject closestObj = null;
            double min = 1000;
            foreach (WorldObject currObject in worldObjects)
            {
                foreach (Point currPoint in GetPoints(currObject))
                {
                    double current = Distance(car.X, car.Y, currPoint);
                    if (current < min)
                    {
                        min = current;
                        closestObj = currObject;
                    }
                }
            }

            return closestObj;
        }


        ///
        /// <summary>
        /// Calculate the distance between the car 'current' point and the roadside found point
        /// with Pythagorean theorem.
        /// </summary>
        /// <param name="carX">Current car X coordinate.</param>
        /// <param name="carY">Current car Y coordinate.</param>
        /// <param name="road">Roadside coordinates.</param>
        /// <returns>Distance between the car and the road.</returns>
        public static double Distance(int carX, int carY, Point road)
        {
            int power = 2;
            return Math.Sqrt(Math.Pow(carX - road.X, power) + Math.Pow(carY - road.Y, power));
        }

        private static List<worldObject> LoadJson()
        {
            using (StreamReader r = new StreamReader($"AutomatedCar.Assets.reference_points.json"))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<List<worldObject>>(json);
            }
        }
    }

    internal class worldObject
    {
        public string Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

    }
}

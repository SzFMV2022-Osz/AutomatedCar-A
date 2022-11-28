namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    public class LaneKeepingAutomatic : SystemComponent
    {
        private List<WorldObject> roadLanes;
        private WorldObject world;
        private AutomatedCar car;
        private LaneKeepingPacket laneKeepingPacket;
        private List<Point> roadPoints;
        // private LKAValidation validation;

        private int x = 2;    // I use this number to 'calibrate' at which point the steer wheel needs to be used.
        private int currentRPM;

        private static IList<worldObject> ReferencePoints = LoadReferencePoints();

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
            this.roadLanes = (List<WorldObject>)this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road);
            this.roadPoints = GetPoints(this.world);

            // This packet will be implemented in other way ...
            if (this.laneKeepingPacket.LaneKeepingStatus == LaneKeepingStatus.Activated)
            {
                //if(!validation.MustBeTurnedOff(roadLanes, (int)World.Instance.ControlledCar.X, (int)World.Instance.ControlledCar.Y))
                //{
                    //if (validation.CanBeTurnedOn(roadLanes, (int)World.Instance.ControlledCar.X, (int)World.Instance.ControlledCar.Y))
                    //{
                        this.Corrigate();
                    //}
                //}
            }
        }

        /// <summary>
        /// Corrigate the car positon from the side of the lane toward to the middle.
        /// </summary>
        public void Corrigate()
        {
            WorldObject closestObject = ;   // Needs to be implemented to get somehow the closest object values.
                                            // Needs to be calculated from the roadLanes and the car current position to know where we need to turn.

            if (this.virtualFunctionBus.PowertrainPacket.CurrentGear == "D")
            {
                this.RotateCar(closestObject);
            }
        }

        private void RotateCar(WorldObject closestObject)
        {
            this.virtualFunctionBus.InputPacket.PedalState = Pedals.Gas;

            // The steering intervention depends on the car orientation and the closest object orientation.
            if (this.car.Rotation > closestObject.Rotation + this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Left;
            }
            else if (this.car.Rotation < closestObject.Rotation - this.x)
            {
                this.virtualFunctionBus.InputPacket.SteeringState = SteeringState.Right;
            }

            throw new NotImplementedException();
        }

        public static List<Point> GetPoints(WorldObject worldObject)
        {
            Point refPoint = new (0, 0);
            if (ReferencePoints.Any(r => r.Type + ".png" == worldObject.Filename))
            {
                worldObject currRefPoint = ReferencePoints.Where(r => r.Type + ".png" == worldObject.Filename).FirstOrDefault();
                refPoint = new (currRefPoint.X, currRefPoint.Y);
            }

            List<Point> points = new () { new Point(worldObject.X, worldObject.Y) };
            foreach (PolylineGeometry currGeometry in worldObject.Geometries)
            {
                foreach (Point currPoint in currGeometry.Points)
                {
                    points.Add(new Point(currPoint.X + worldObject.X - refPoint.X, currPoint.Y + worldObject.Y - refPoint.Y));
                }
            }

            return points;
        }

        private static IList<worldObject> LoadReferencePoints()
        {
            string jsonString = new StreamReader(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"AutomatedCar.Assets.reference_points.json")).ReadToEnd();
            return JsonConvert.DeserializeObject<List<worldObject>>(jsonString);
        }

    }

    internal class worldObject
    {
        public string Type { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

    }
}

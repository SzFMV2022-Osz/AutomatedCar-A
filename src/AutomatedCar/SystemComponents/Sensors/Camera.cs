namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;

    public class Camera : Sensor
    {
        private WorldObject cameraViewField;
        private PolylineGeometry viewFieldGeometry;

        public Camera(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            var deg = 60;
            var dist = 80;
            this.vision = SensorVision.CalculateVision(dist, deg, new Point(0, 0));
            this.virtualFunctionBus.CameraPacket = new SensorPacket();
        }

        private void InitCameraViewField()
        {
            var car = World.Instance.ControlledCar;
            var cameraPos = new Point(
                x: car.RotationPoint.X,
                y: car.Geometry.Bounds.Top + 50);

            this.cameraViewField = new WorldObject(0, 0, string.Empty);
            var geometries = new Avalonia.Media.PolylineGeometry();
            geometries.Points.Add(vision.SensorPos + cameraPos);
            geometries.Points.Add(vision.Left + cameraPos);
            geometries.Points.Add(vision.Right + cameraPos);

            this.cameraViewField.RawGeometries.Add(geometries);
            this.cameraViewField.RotationPoint = car.RotationPoint;
        }

        public override void Process()
        {
            if (this.cameraViewField is null)
            {
                this.InitCameraViewField();
            }

            // Update camera view field
            this.cameraViewField.X = World.Instance.ControlledCar.X;
            this.cameraViewField.Y = World.Instance.ControlledCar.Y;
            this.cameraViewField.Rotation = World.Instance.ControlledCar.Rotation;
            this.viewFieldGeometry = Helpers.CollisionDetection.TransformRawGeometry(this.cameraViewField);

            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            var objs = this.GetWorldObjects();
            return objs.Where(obj => (obj.WorldObjectType == WorldObjectType.Road || obj.WorldObjectType == WorldObjectType.RoadSign) && this.IsRelevant(obj)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }

        private bool IsRelevant(WorldObject obj)
        {
            var isInTriangle = false;
            var objPolys = CollisionDetection.TransformRoadRawGeometry(obj);
            var cameraTriangle = new Tuple<Point, Point, Point>(
                this.viewFieldGeometry.Points[0],
                this.viewFieldGeometry.Points[1],
                this.viewFieldGeometry.Points[2]
                );

            foreach (var objPoly in objPolys)
            {
                for (int i = 0; i < objPoly.Points.Count && !isInTriangle; ++i)
                {
                    isInTriangle = CollisionDetection.PointInTriangle(objPoly.Points[i], cameraTriangle);
                }

                if (!isInTriangle)
                {
                    isInTriangle = CollisionDetection.BoundingBoxesCollide(this.viewFieldGeometry, objPoly, 1);
                }

                if (isInTriangle)
                {
                    break; // :)
                }
            }

            return isInTriangle;
        }

        protected List<WorldObject> FilterRoad()
        {
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road).ToList();
        }

        protected WorldObject NearestWorldObject()
        {
            this.OrderByDistance();
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.FirstOrDefault();
        }

        private void OrderByDistance()
        {
            // distance = sqrt((x2-x1)^2+(y2-y1)^2)
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.OrderBy(obj => Math.Sqrt(Math.Pow(obj.X - this.vision.SensorPos.X, 2) + Math.Pow(obj.Y - this.vision.SensorPos.Y, 2))).ToList();
        }
    }
}

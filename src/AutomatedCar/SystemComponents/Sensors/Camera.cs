namespace AutomatedCar.SystemComponents.Sensors
{
    using AutomatedCar.Helpers;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Numerics;

    public class Camera : Sensor
    {
        private WorldObject CameraViewField;
        private PolylineGeometry viewFieldGeometry;

        public Camera(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            double deg = 60;
            int dist = 80;
            this.vision = SensorVision.CalculateVision(dist, deg, new Point(0, 0));
        }

        private void InitCameraViewField()
        {
            var car = World.Instance.ControlledCar;
            Point cameraPos = new Point(
                x: car.RotationPoint.X,
                y: car.Geometry.Bounds.Top + 50);

            CameraViewField = new WorldObject(0, 0, string.Empty);
            var geometries = new Avalonia.Media.PolylineGeometry();
            geometries.Points.Add(vision.SensorPos + cameraPos);
            geometries.Points.Add(vision.Left + cameraPos);
            geometries.Points.Add(vision.Right + cameraPos);

            CameraViewField.RawGeometries.Add(geometries);
            CameraViewField.RotationPoint = car.RotationPoint;
        }

        public override void Process()
        {
            if (CameraViewField is null)
            {
                InitCameraViewField();
            }

            // Update camera view field
            CameraViewField.X = World.Instance.ControlledCar.X;
            CameraViewField.Y = World.Instance.ControlledCar.Y;
            CameraViewField.Rotation = World.Instance.ControlledCar.Rotation;
            this.viewFieldGeometry = Helpers.CollisionDetection.TransformRawGeometry(CameraViewField);
            this.SaveWorldObjectsToPacket();
        }

        protected override List<WorldObject> FilterRelevantWorldObjects()
        {
            List<WorldObject> objs = this.GetWorldObjects();
            return objs.Where(obj => (obj.WorldObjectType == WorldObjectType.Road || obj.WorldObjectType == WorldObjectType.RoadSign) && this.IsRelevant(obj)).ToList();
        }

        protected override void SaveWorldObjectsToPacket()
        {
            this.virtualFunctionBus.CameraPacket = new SensorPacket();
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.FilterRelevantWorldObjects();
        }

        private bool IsRelevant(WorldObject obj)
        {
            var objPoly = CollisionDetection.TransformRawGeometry(obj);

            bool isInTriangle = false;

            foreach (var geom in CollisionDetection.TransformRoadRawGeometry(obj))
            {
                isInTriangle |= CollisionDetection.BoundingBoxesCollide(viewFieldGeometry, geom, 1);
            }

            return isInTriangle;
        }

        protected List<WorldObject> FilterRoad()
        {
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.Where(obj => obj.WorldObjectType == WorldObjectType.Road).ToList();
        }

        protected WorldObject NearestWorldObject()
        {
            OrderByDistance();
            return this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.FirstOrDefault();
        }

        private void OrderByDistance()
        {
            //distance = sqrt((x2-x1)^2+(y2-y1)^2)
            this.virtualFunctionBus.CameraPacket.RelevantWorldObjs = this.virtualFunctionBus.CameraPacket.RelevantWorldObjs.OrderBy(obj => Math.Sqrt(Math.Pow(obj.X - this.vision.SensorPos.X, 2) + Math.Pow(obj.Y - this.vision.SensorPos.Y, 2))).ToList();
        }
    }
}

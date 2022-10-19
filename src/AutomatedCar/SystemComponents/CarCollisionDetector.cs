namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Media;
    using System.Collections.Generic;
    using System.Linq;

    public class CarCollisionDetector : SystemComponent
    {
        private CollisionPacket packet;
        private IEnumerable<WorldObject> collidableWorldObjects;

        public CarCollisionDetector(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.packet = new CollisionPacket();
            this.collidableWorldObjects = World.Instance.WorldObjects
                                        .Where(obj => obj.Collideable
                                        && obj != World.Instance.ControlledCar);
        }

        public override void Process()
        {
            var collisions = this.collidableWorldObjects
                            .Where(obj => CollisionCheck(World.Instance.ControlledCar, obj));

            var dynamicWorldObjects = collisions
                           .Where(obj => obj.WorldObjectType == WorldObjectType.Car
                           || obj.WorldObjectType == WorldObjectType.Pedestrian);

            var staticWorldObjects = collisions
                       .Where(obj => obj.WorldObjectType == WorldObjectType.Building
                       || obj.WorldObjectType == WorldObjectType.Tree
                       || obj.WorldObjectType == WorldObjectType.RoadSign);

            this.packet.Collided = collisions.Any();
            this.packet.CollisionsWithNPCs = dynamicWorldObjects;
            this.packet.CollisionsWithStaticObjects = staticWorldObjects;
        }

        private bool CollisionCheck(WorldObject car, WorldObject obj)
        {
            PolylineGeometry transformedCarGeometry = TransformGeometry(car);
            PolylineGeometry transformedObjGeometry = TransformGeometry(obj);

            return Helpers.CollisionDetection.BoundingBoxesCollide(transformedCarGeometry,
                transformedObjGeometry, 1);
        }

        // This will be moved to Helpers later
        private PolylineGeometry TransformGeometry(WorldObject obj)
        {
            PolylineGeometry transformedGeometry = new PolylineGeometry(); 
            foreach (Point point in obj.RawGeometries.First().Points)
            {
                transformedGeometry.Points.Add(
                    new Point(point.X + obj.X, point.Y + obj.Y));
            }
            return transformedGeometry;
        }
    }
}

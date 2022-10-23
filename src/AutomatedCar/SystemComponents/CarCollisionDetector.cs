namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;
    using Avalonia;
    using Avalonia.Data.Converters;
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
                .Where(obj => obj.Collideable &&
                obj != World.Instance.ControlledCar);
        }

        public override void Process()
        {
            var collisions = this.collidableWorldObjects
                .Where(obj => this.CollisionCheck(World.Instance.ControlledCar, obj));

            var dynamicWorldObjects = collisions
                .Where(obj => obj.WorldObjectType == WorldObjectType.Car ||
                obj.WorldObjectType == WorldObjectType.Pedestrian);

            var staticWorldObjects = collisions
                .Where(obj => obj.WorldObjectType == WorldObjectType.Building ||
                obj.WorldObjectType == WorldObjectType.Tree ||
                obj.WorldObjectType == WorldObjectType.RoadSign);

            this.packet.Collided = collisions.Any();
            this.packet.CollisionsWithNPCs = dynamicWorldObjects;
            this.packet.CollisionsWithStaticObjects = staticWorldObjects;
        }

        private bool CollisionCheck(AutomatedCar car, WorldObject obj)
        {
            var carGeometry = Helpers.CollisionDetection.TransformRawGeometry(car);
            var objGeometry = Helpers.CollisionDetection.TransformRawGeometry(obj);

            return Helpers.CollisionDetection.BoundingBoxesCollide(carGeometry, objGeometry, 1);
        }
    }
}

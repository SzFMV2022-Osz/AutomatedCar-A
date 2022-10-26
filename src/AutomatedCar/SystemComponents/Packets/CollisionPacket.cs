namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class CollisionPacket : ReactiveObject, IReadOnlyCollisionPacket
    {
        private bool collided;
        private IEnumerable<WorldObject> collisionWithNPCs;
        private IEnumerable<WorldObject> collisionWithStaticObjects;
        private ObservableCollection<WorldObject> collidedObjects;

        public bool Collided
        {
            get => this.collided;
            set => this.RaiseAndSetIfChanged(ref this.collided, value);
        }

        public IEnumerable<WorldObject> CollisionsWithNPCs
        {
            get => this.collisionWithNPCs;
            set => this.RaiseAndSetIfChanged(ref this.collisionWithNPCs, value);
        }

        public IEnumerable<WorldObject> CollisionsWithStaticObjects
        {
            get => this.collisionWithStaticObjects;
            set => this.RaiseAndSetIfChanged(ref this.collisionWithStaticObjects, value);
        }

        public ObservableCollection<WorldObject> CollidedObjects
        {
            get => this.collidedObjects;
            set => this.RaiseAndSetIfChanged(ref this.collidedObjects, value);
        }
    }
}

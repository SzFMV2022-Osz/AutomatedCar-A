namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;

    public interface IReadOnlyCollisionPacket
    {
        bool Collided { get; }

        IEnumerable<WorldObject> CollisionsWithNPCs { get; }

        IEnumerable<WorldObject> CollisionsWithStaticObjects { get; }
    }
}

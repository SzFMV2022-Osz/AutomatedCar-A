namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using System.Collections.Generic;

    public interface IReadOnlyCollisionPacket
    {
        bool Collided { get; }
        IEnumerable<WorldObject> CollisionsWithNPCs { get; }
        IEnumerable<WorldObject> CollisionsWithStaticObjects { get; }
    }
}

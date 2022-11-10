namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;

    public interface IRadarPacket : ISensorPacket
    {
        WorldObject ClosestInLane { get; set; }

        WorldObject Closest { get; set; }
    }
}

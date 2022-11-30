namespace AutomatedCar.SystemComponents.Packets
{
    public interface ILKAPacket
    {
        bool LaneKeepingAvailable { get; set; }
        bool LaneKeepingStatus { get; set; }
    }
}
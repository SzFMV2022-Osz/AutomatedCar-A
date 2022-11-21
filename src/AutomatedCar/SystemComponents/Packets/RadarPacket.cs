namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;

    internal class RadarPacket : SensorPacket, IRadarPacket
    {
        private WorldObject closestInLane;
        private WorldObject closest;

        public WorldObject ClosestInLane
        {
            get => this.closestInLane;
            set => this.RaiseAndSetIfChanged(ref this.closestInLane, value);
        }

        public WorldObject Closest
        {
            get => this.closest;
            set => this.RaiseAndSetIfChanged(ref this.closest, value);
        }
    }
}

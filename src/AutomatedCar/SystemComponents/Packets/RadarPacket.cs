namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.Models;
    using Avalonia;
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
        private Rect frontalRadarArea;

        private Dictionary<WorldObject, WorldObjectTracker> objectTrackingDatas;

        public WorldObject ClosestInLane
        {
            get => this.closestInLane;
            set
            {
                this.RaiseAndSetIfChanged(ref this.closestInLane, null); // ugly fix for refreshing UI, but it works
                this.RaiseAndSetIfChanged(ref this.closestInLane, value);
            }
        }

        public WorldObject Closest
        {
            get => this.closest;
            set
            {
                this.RaiseAndSetIfChanged(ref this.closest, null); // same here
                this.RaiseAndSetIfChanged(ref this.closest, value);
            }
        }

        public Dictionary<WorldObject, WorldObjectTracker> ObjectTrackingDatas
        {
            get => this.objectTrackingDatas;
            set => this.objectTrackingDatas = value;
        }

        public Rect FrontalRadarArea
        {
            get => this.frontalRadarArea;
            set => this.frontalRadarArea = value;
        }
    }
}

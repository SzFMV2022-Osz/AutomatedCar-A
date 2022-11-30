namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();

        public IReadOnlyDummyPacket DummyPacket { get; set; }

        public IRadarPacket RadarPacket { get; set; }

        public ISensorPacket CameraPacket { get; set; }

        public IReadOnlyCollisionPacket CollisionPacket { get; set; }

        public IPowertrainPacket PowertrainPacket { get; set; }

        public ILaneKeepingPacket LeaneKeepingPacket { get; set; }

        public InputPacket InputPacket { get; set; }

        public CruiseControlPacket CruiseControlPacket { get; set; }

        public IReadOnlyAEBPacket AEBPacket { get; set; }

        public void RegisterComponent(SystemComponent component)
        {
            this.components.Add(component);
        }

        protected override void Tick()
        {
            foreach (SystemComponent component in this.components)
            {
                component.Process();
            }
        }
    }
}
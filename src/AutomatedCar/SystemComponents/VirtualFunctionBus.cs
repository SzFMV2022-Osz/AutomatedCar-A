namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.SystemComponents.Packets;
    using System.Collections.Generic;

    public class VirtualFunctionBus : GameBase
    {
        private List<SystemComponent> components = new List<SystemComponent>();

        public IReadOnlyDummyPacket DummyPacket { get; set; }

        public ISensorPacket RadarPacket { get; set; }

        public IReadOnlyCollisionPacket CollisionPacket { get; set; }

        public ICarCoordinatesPacket CarCoordinatesPacket { get; set; }

        public IPowerTrainPacketForChangeSpeed PowerTrainPacketForSpeed { get; set; }

        public IPowerTrainPacketForChangeGearshiftState PowerTrainPacketForGearshift { get; set; }

        public IPowerTrainPacketForChangeSteering PowerTrainPacketForSteering { get; set; }

        public IMoveObject MoveObject { get; set; }

        public IDasboardPacket DasboardPacket { get; set; }

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
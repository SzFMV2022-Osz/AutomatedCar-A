namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models.NPC;

    public class MoveComponent : SystemComponent
    {
        private INPC npc;

        public MoveComponent(VirtualFunctionBus virtualFunctionBus, INPC npc)
            : base(virtualFunctionBus)
        {
            this.npc = npc;
        }

        public override void Process()
        {
            this.npc.Move();
        }
    }
}

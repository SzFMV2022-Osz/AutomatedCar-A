namespace AutomatedCar.SystemComponents
{
    using AutomatedCar.Models.NPC;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MoveComponent : SystemComponent
    {
        private INPC npc;
        public MoveComponent(VirtualFunctionBus virtualFunctionBus, INPC npc) : base(virtualFunctionBus)
        {
            this.npc = npc;
        }

        public override void Process()
        {
            npc.Move();
        }
    }
}

namespace AutomatedCar.Models.NPC
{
    using global::AutomatedCar.SystemComponents;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    abstract class NPC : WorldObject
    {
        protected NPC(int x, int y, string filename, int zindex = 1, bool collideable = false, WorldObjectType worldObjectType = WorldObjectType.Other) : base(x, y, filename, zindex, collideable, worldObjectType)
        {
            this.VirtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
        }

        public int Speed { get; set; }
        protected MoveComponent moveComponent;

        protected abstract void Move();
        
        public VirtualFunctionBus VirtualFunctionBus { get; }


        /// <summary>Starts the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.VirtualFunctionBus.Start();
        }

        /// <summary>Stops the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.VirtualFunctionBus.Stop();
        }
    }
}

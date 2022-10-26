namespace AutomatedCar.Models.NPC
{
    using global::AutomatedCar.SystemComponents;
    using Route;

    abstract class NPC : WorldObject
    {
        protected NPC(Route route, string filename, int zindex = 1, bool collideable = false, WorldObjectType worldObjectType = WorldObjectType.Other)
            : base(route.CurrentPoint.X, route.CurrentPoint.Y, filename, zindex, collideable, worldObjectType)
        {
            this.VirtualFunctionBus = new VirtualFunctionBus();
            this.ZIndex = 10;
            this.Route = route;
        }

        public int Speed { get; set; }

        protected VirtualFunctionBus VirtualFunctionBus { get; set; }

        protected Route Route { get; }

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

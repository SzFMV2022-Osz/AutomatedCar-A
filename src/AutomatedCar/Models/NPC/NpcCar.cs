namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the NPC car.
    /// </summary>
    internal class NpcCar : NPC
    {
        public NpcCar(int x, int y, string filename, int zindex = 1)
            : base(x, y, filename, zindex, true, WorldObjectType.Car)
        {
        }

        /// <inheritdoc/>
        protected override void Move()
        {
        }
    }
}

using System;

namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::AutomatedCar.SystemComponents;

    /// <summary>
    /// Represents the NPC car.
    /// </summary>
    internal class NpcPerson : NPC, INPC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCar"/> class.
        /// </summary>
        /// <param name="x">X coordinate of current position.</param>
        /// <param name="y">Y coordinate of current position.</param>
        /// <param name="filename">.</param>
        public NpcPerson(int x, int y, string filename)
            : base(x, y, filename, 10, true, WorldObjectType.Car)
        {
            this.MoveComponent = new MoveComponent(this.VirtualFunctionBus, this);
            this.VirtualFunctionBus.RegisterComponent(this.MoveComponent);
        }

        /// <inheritdoc/>
        public MoveComponent MoveComponent { get; set; }

        /// <inheritdoc/>
        public void Move()
        {
        }
    }
}


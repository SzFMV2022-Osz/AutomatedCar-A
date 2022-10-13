// <copyright file="NpcCar.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

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
    internal class NpcCar : NPC, INPC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCar"/> class.
        /// </summary>
        /// <param name="x">X coordinate of current position.</param>
        /// <param name="y">Y coordinate of current position.</param>
        /// <param name="filename">.</param>
        public NpcCar(int x, int y, string filename)
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
            int x = 300;
            int y = 600;
            int width = Math.Abs(this.X - x);
            int height = Math.Abs(this.Y - y);

            if (this.Y > y)
            {
                this.Y = this.Y - (height + width) / width;
            }
            if (this.Y < y)
            {
                this.Y = this.Y + (height + width) / width;
            }
            if (this.X > x)
            {
                this.X = this.X - (width + height) / height;
            }
            if (this.X < x)
            {
                this.X = this.X + (width + height) / height;
            }
        }
    }
}

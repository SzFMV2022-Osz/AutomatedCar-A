// <copyright file="Locator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Locator
{
    using AutomatedCar.Models;
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class moves the car.
    /// </summary>
    internal class Locator : SystemComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Locator"/> class.
        /// </summary>
        /// <param name="vfb">Bus.</param>
        /// <param name="objectToMove">Object to move.</param>
        public Locator(VirtualFunctionBus vfb, WorldObject objectToMove)
            : base(vfb)
        {
            this.ObjectToMove = objectToMove;
        }

        private WorldObject ObjectToMove { get; set; }

        /// <summary>
        /// Process.
        /// </summary>
        public override void Process()
        {
            if (this.virtualFunctionBus.MoveObject is not null)
            {
                Vector vector = this.virtualFunctionBus.MoveObject.Vector;
                this.ObjectToMove.X += (int)vector.X;
                this.ObjectToMove.Y += (int)vector.Y;
            }
        }
    }
}

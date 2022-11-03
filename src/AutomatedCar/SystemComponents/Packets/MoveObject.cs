// <copyright file="MoveObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using Avalonia;

    /// <summary>
    /// Move object packet.
    /// </summary>
    public class MoveObject : IMoveObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveObject"/> class.
        /// </summary>
        /// <param name="vector">Vector to move.</param>
        /// <param name="rotation">new rotaion.</param>
        public MoveObject(Vector vector, double rotation)
        {
            this.Vector = vector;
            this.Rotation = rotation;
        }

        /// <summary>
        /// Gets vector to move.
        /// </summary>
        public Vector Vector { get; private set; }

        /// <summary>
        /// Gets rotation.
        /// </summary>
        public double Rotation { get; private set; }
    }
}

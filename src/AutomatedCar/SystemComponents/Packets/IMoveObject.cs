// <copyright file="IMoveObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using Avalonia;

    /// <summary>
    /// Move object.
    /// </summary>
    public interface IMoveObject
    {
        /// <summary>
        /// Gets vector to move.
        /// </summary>
        public Vector Vector { get; }
    }
}

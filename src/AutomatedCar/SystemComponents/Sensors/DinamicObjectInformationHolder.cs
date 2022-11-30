// <copyright file="DinamicObjectInformationHolder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Dinamic object predicated position and rotation.
    /// </summary>
    internal class DinamicObjectInformationHolder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DinamicObjectInformationHolder"/> class.
        /// </summary>
        /// <param name="position">Object position.</param>
        /// <param name="rotation">Object rotation.</param>
        public DinamicObjectInformationHolder(Point position, double rotation)
        {
            this.Position = position;
            this.Rotation = rotation;
        }

        /// <summary>
        /// Gets the object position.
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// Gets the object rotation.
        /// </summary>
        public double Rotation { get; private set; }
    }
}

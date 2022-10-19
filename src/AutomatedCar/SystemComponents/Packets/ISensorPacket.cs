// <copyright file="ISensorPacket.cs" company="TEAM-A2">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;

    /// <summary>
    /// A service that provides a collection of type <see cref="WorldObject"/>.
    /// </summary>
    {
        /// <summary>
        /// Gets or sets a collection of type WorldObject.
        /// </summary>
        IEnumerable<WorldObject> RelevantWorldObjects { get; set; }
    }
}

// <copyright file="SensorPacket.cs" company="TEAM-A2">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using ReactiveUI;

    /// <summary>
    /// An implementation of <see cref="ISensorPacket"/>.
    /// </summary>
    internal class SensorPacket : ReactiveObject, ISensorPacket
    {
        private IEnumerable<WorldObject> relevantWorldObjects;

        /// <summary>
        /// Gets or sets a collection of type <see cref="WorldObject"/>.
        /// </summary>
        public IEnumerable<WorldObject> RelevantWorldObjects
        {
            get => this.RelevantWorldObjects;
            set => this.RaiseAndSetIfChanged(ref this.relevantWorldObjects, value);
        }
    }
}

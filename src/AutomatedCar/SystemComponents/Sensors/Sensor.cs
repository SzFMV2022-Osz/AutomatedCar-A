// <copyright file="Sensor.cs" company="TEAM-A2">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Sensors
{
    using System.Collections.Generic;
    using AutomatedCar.Models;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Abstract Sensor class with basic abstractions.
    /// </summary>
    internal abstract class Sensor : SystemComponent
    {
        private readonly ISensorPacket sensorPacket;

        /// <summary>
        /// Initializes a new instance of the <see cref="Sensor"/> class.
        /// </summary>
        /// <param name="sensorPacket"><see cref="ISensorPacket"/> implementation.</param>
        /// <param name="virtualFunctionBus">An instance of <see cref="VirtualFunctionBus"/>.</param>
        public Sensor(ISensorPacket sensorPacket, VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.sensorPacket = sensorPacket;
        }

        /// <summary>
        /// Filters relevant <see cref="WorldObject"/> items.
        /// </summary>
        /// <param name="worldObjects">A collection of type <see cref="WorldObject"/>.</param>
        /// <returns>A collection of WorldObjects.</returns>
        protected abstract IEnumerable<WorldObject> FilterRelevantWorldObjects(IEnumerable<WorldObject> worldObjects);

        /// <summary>
        /// Saves the collection of <see cref="WorldObject"/>.
        /// </summary>
        /// <param name="worldObjects">A collection of type <see cref="WorldObject"/>.</param>
        protected abstract void SaveWorldObjectsToPacket(IEnumerable<WorldObject> worldObjects);

        private static AutomatedCar GetAutomatedCar()
        {
            return World.Instance.ControlledCar;
        }

        private IEnumerable<WorldObject> GetWorldObjects()
        {
            return World.Instance.WorldObjects;
        }
    }
}

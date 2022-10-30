// <copyright file="IDasboardPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Packet for dasboard.
    /// </summary>
    public interface IDasboardPacket
    {
        /// <summary>
        /// gets rpm of the engine.
        /// </summary>
        public float RPM { get; }

        /// <summary>
        /// Gets Speed of the veicle.
        /// </summary>
        public float Speed { get; }

        /// <summary>
        /// Gets throtle pedal %.
        /// </summary>
        public float Throtle { get; }

        /// <summary>
        /// Gets break pedal %.
        /// </summary>
        public float Break { get; }

        /// <summary>
        /// Gets the gearshift state.
        /// </summary>
        public string ShiftState { get; }
    }
}

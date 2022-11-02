// <copyright file="InputPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;

    /// <summary>
    /// Packet for communication between the user and powertrain.
    /// </summary>
    public class InputPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputPacket"/> class.
        /// </summary>
        public InputPacket()
        {
            this.SteeringState = SteeringState.Center;
            this.PedalState = Pedals.Empty;
            this.GearState = Gears.Steady;
            this.IsGearStateJustChanged = false;
        }

        /// <summary>
        /// Gets or sets the state of the steering.
        /// </summary>
        public SteeringState SteeringState { get; set; }

        /// <summary>
        /// Gets or sets the state of the pedal.
        /// </summary>
        public Pedals PedalState { get; set; }

        /// <summary>
        /// Gets or sets the state of the gearbox.
        /// </summary>
        public Gears GearState { get; set; }

        /// <summary>
        /// Gets or sets if the gear state is just changed.
        /// </summary>
        public bool IsGearStateJustChanged { get; set; }

        /// <summary>
        /// For logging.
        /// </summary>
        /// <returns>A string to log.</returns>
        public override string ToString()
        {
            return $"{this.SteeringState.ToString()} - {this.PedalState.ToString()} - {this.GearState.ToString()} - GearStateJustChanged: {this.IsGearStateJustChanged}";
        }
    }
}

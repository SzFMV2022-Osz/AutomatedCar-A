// <copyright file="Manager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.InputHandler
{
    using AutomatedCar.Models.InputManager.Messenger;

    /// <summary>
    /// This class manages the inputs.
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// This is a controlMessenger instance.
        /// </summary>
        private static ControlMessenger controlMessenger = new ControlMessenger();

        /// <summary>
        /// Send turning left input to powertrain.
        /// </summary>
        public static void TurnLeft()
        {
            controlMessenger.Steering = Steering.Left;
        }

        /// <summary>
        /// Send turning right input to powertrain.
        /// </summary>
        public static void TurnRight()
        {
            controlMessenger.Steering = Steering.Right;
        }

        /// <summary>
        /// Send turning center input to powertrain.
        /// </summary>
        public static void TurnToCenter()
        {
            controlMessenger.Steering = Steering.Center;
        }

        /// <summary>
        /// Send shifting down input to powertrain.
        /// </summary>
        public static void ShiftDown()
        {
            controlMessenger.Gear = Gears.ShiftDown;
        }

        /// <summary>
        /// Send shifting up input to powertrain.
        /// </summary>
        public static void ShiftUp()
        {
            controlMessenger.Gear = Gears.ShiftUp;
        }

        /// <summary>
        /// Send brake input to powertrain.
        /// </summary>
        public static void BrakePedal()
        {
            controlMessenger.Pedal = Pedals.Brake;
        }

        /// <summary>
        /// Send gas left input to powertrain.
        /// </summary>
        public static void GasPedal()
        {
            controlMessenger.Pedal = Pedals.Gas;
        }

        /// <summary>
        /// Send empty input to powertrain.
        /// </summary>
        public static void Empty()
        {
            controlMessenger.Pedal = Pedals.Empty;
        }
    }
}

// <copyright file="Manager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.InputHandler
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;

    /// <summary>
    /// This class manages the inputs.
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// This is a controlMessenger instance.
        /// </summary>
        public static ControlMessenger controlMessenger = new ControlMessenger();

        /// <summary>
        /// Send turning left input to powertrain.
        /// </summary>
        public static void TurnLeft()
        {
            controlMessenger.Steering = SteeringState.Left;
        }

        /// <summary>
        /// Send turning right input to powertrain.
        /// </summary>
        public static void TurnRight()
        {
            controlMessenger.Steering = SteeringState.Right;
        }

        /// <summary>
        /// Send turning center input to powertrain.
        /// </summary>
        public static void TurnToCenter()
        {
            controlMessenger.Steering = SteeringState.Center;
        }

        /// <summary>
        /// Send shifting down input to powertrain.
        /// </summary>
        public static void ShiftDown()
        {
            controlMessenger.Gear = Gears.ShiftDown;

            // controlMessenger.Gear = Gears.Steady;
        }

        /// <summary>
        /// Send shifting up input to powertrain.
        /// </summary>
        public static void ShiftUp()
        {
            controlMessenger.Gear = Gears.ShiftUp;

            // controlMessenger.Gear = Gears.Steady;
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

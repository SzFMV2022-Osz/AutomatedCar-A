//-----------------------------------------------------------------------
// <copyright file="ControlEnums.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace AutomatedCar.Models.InputManager.Messenger
{
    /// <summary>
    /// Enum for the steering wheel.
    /// </summary>
    public enum Steering
    {
        /// <summary>
        /// Left position of the steering wheel
        /// </summary>
        Left,

        /// <summary>
        /// Center position of the steering wheel
        /// </summary>
        Center,

        /// <summary>
        /// Right position of the steering wheel
        /// </summary>
        Right,
    }

    /// <summary>
    /// Enum for the pedals.
    /// </summary>
    public enum Pedals
    {
        /// <summary>
        /// Gas pedal
        /// </summary>
        Gas,

        /// <summary>
        /// Brake pedal
        /// </summary>
        Brake,
    }

    /// <summary>
    /// Enum for the gears.
    /// </summary>
    public enum Gears
    {
        /// <summary>
        /// Park position
        /// </summary>
        Park,

        /// <summary>
        /// Reverse position
        /// </summary>
        Reverse,

        /// <summary>
        /// Neutral position
        /// </summary>
        Neutral,

        /// <summary>
        /// Drive position
        /// </summary>
        Drive,
    }
}

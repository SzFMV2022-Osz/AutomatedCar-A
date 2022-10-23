//-----------------------------------------------------------------------
// <copyright file="ControlEnums.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace AutomatedCar.SystemComponents.InputManager.Messenger
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

        /// <summary>
        /// No pedal movement
        /// </summary>
        Empty,
    }

    /// <summary>
    /// Enum for the gears.
    /// </summary>
    public enum Gears
    {
        /// <summary>
        /// Shift up one value
        /// </summary>
        ShiftUp,

        /// <summary>
        /// Shift down one value
        /// </summary>
        ShiftDown,

        /// <summary>
        /// No gear shifting
        /// </summary>
        Steady,
    }
}

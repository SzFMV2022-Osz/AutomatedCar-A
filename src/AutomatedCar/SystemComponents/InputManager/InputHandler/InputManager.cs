// <copyright file="InputManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager.InputHandler
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// InputManager manages the input packets for the powertrain.
    /// </summary>
    public class InputManager : SystemComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// Constructor for the InputManager, which gives a virtualFunctionBus instance to the base system components constructor.
        /// </summary>
        /// <param name="virtualFunctionBus">A virtualFunctionBus instance.</param>
        public InputManager(VirtualFunctionBus virtualFunctionBus)
            : base(virtualFunctionBus)
        {
            this.InputPacket = new InputPacket();
            virtualFunctionBus.InputPacket = this.InputPacket;

            ControlMessenger.Instance.SteeringEventHandler += this.OnSteering;
            ControlMessenger.Instance.PedalEventHandler += this.OnPedal;
            ControlMessenger.Instance.GearboxEventHandler += this.OnGearbox;
        }

        /// <summary>
        /// Gets or sets a packet for the powertrain.
        /// </summary>
        public InputPacket InputPacket { get; set; }

        /// <summary>
        /// Sets the packet for steering.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="eventArgs">Custom event args.</param>
        public void OnSteering(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Steering)
            {
                case SteeringState.Left:
                    this.InputPacket.SteeringState = SteeringState.Left;
                    break;
                case SteeringState.Right:
                    this.InputPacket.SteeringState = SteeringState.Right;
                    break;
                case SteeringState.Center:
                    this.InputPacket.SteeringState = SteeringState.Center;
                    break;
            }
        }

        /// <summary>
        /// Sets the packet for pedals.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="eventArgs">Custom event args.</param>
        public void OnPedal(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Pedal)
            {
                case Pedals.Gas:
                    this.InputPacket.PedalState = Pedals.Gas;
                    break;
                case Pedals.Empty:
                    this.InputPacket.PedalState = Pedals.Empty;
                    break;
                case Pedals.Brake:
                    this.InputPacket.PedalState = Pedals.Brake;
                    break;
            }
        }

        /// <summary>
        /// Sets the packet for gearbox.
        /// </summary>
        /// <param name="sender">Object.</param>
        /// <param name="eventArgs">Custom event args.</param>
        public void OnGearbox(object sender, ControlEventArgs eventArgs)
        {
            switch (eventArgs.Gear)
            {
                case Gears.ShiftUp:
                    this.InputPacket.GearState = Gears.ShiftUp;
                    break;
                case Gears.ShiftDown:
                    this.InputPacket.GearState = Gears.ShiftDown;
                    break;
                case Gears.Steady:
                    this.InputPacket.GearState = Gears.Steady;
                    break;
            }
        }

        private bool IsGearStateJustChanged(Gears newGearState)
        {
            return this.InputPacket.GearState != newGearState;
        }

        public override void Process()
        {
            // IsGearStateJustChanged (todo)

        }
    }
}

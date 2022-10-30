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
            this.CurrentGearState = Gears.Steady;
            this.CurrendPedalState = Pedals.Empty;
            this.CurrentSteeringState = SteeringState.Center;

            ControlMessenger.Instance.SteeringEventHandler += this.OnSteering;
            ControlMessenger.Instance.PedalEventHandler += this.OnPedal;
            ControlMessenger.Instance.GearboxEventHandler += this.OnGearbox;
        }

        /// <summary>
        /// Gets or sets a packet for the powertrain.
        /// </summary>
        public InputPacket InputPacket { get; set; }

        /// <summary>
        /// Gets or sets the next value for the InputPacket.
        /// </summary>
        private Gears CurrentGearState { get; set; }

        /// <summary>
        /// Gets or sets the next value for the InputPacket.
        /// </summary>
        private SteeringState CurrentSteeringState { get; set; }

        /// <summary>
        /// Gets or sets the next value for the InputPacket.
        /// </summary>
        private Pedals CurrendPedalState { get; set; }

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
                    this.CurrentSteeringState = SteeringState.Left;
                    break;
                case SteeringState.Right:
                    this.CurrentSteeringState = SteeringState.Right;
                    break;
                case SteeringState.Center:
                    this.CurrentSteeringState = SteeringState.Center;
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
                    this.CurrendPedalState = Pedals.Gas;
                    break;
                case Pedals.Empty:
                    this.CurrendPedalState = Pedals.Empty;
                    break;
                case Pedals.Brake:
                    this.CurrendPedalState = Pedals.Brake;
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
                    this.CurrentGearState = Gears.ShiftUp;
                    break;
                case Gears.ShiftDown:
                    this.CurrentGearState = Gears.ShiftDown;
                    break;
                case Gears.Steady:
                    this.CurrentGearState = Gears.Steady;
                    break;
            }
        }

        /// <inheritdoc/>
        public override void Process()
        {
            this.InputPacket.IsGearStateJustChanged = this.IsGearStateJustChanged(this.CurrentGearState);
            this.InputPacket.GearState = this.CurrentGearState;
            this.InputPacket.PedalState = this.CurrendPedalState;
            this.InputPacket.SteeringState = this.CurrentSteeringState;
        }

        private bool IsGearStateJustChanged(Gears newGearState)
        {
            return this.InputPacket.GearState != newGearState;
        }
    }
}

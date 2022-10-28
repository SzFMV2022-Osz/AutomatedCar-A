// <copyright file="InputManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Input manager.
    /// </summary>
    internal class InputManager : SystemComponent
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        /// <param name="vsf">Virtual function bus.</param>
        /// <param name="messenger">IMessenger.</param>
        public InputManager(VirtualFunctionBus vsf, IMessenger messenger)
            : base(vsf)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Do process.
        /// </summary>
        public override void Process()
        {
            if (Keyboard.IsKeyDown(Avalonia.Input.Key.Up))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.PedalStates.Throtle);
                Keyboard.Keys.Remove(Avalonia.Input.Key.Up);
            }
            else if (Keyboard.IsKeyDown(Avalonia.Input.Key.Down))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.PedalStates.Break);
            }
            else
            {
                //this.messenger.SendMessageToPowertrain(Models.PowerTrain.PedalStates.None);
            }

            if (Keyboard.IsKeyDown(Avalonia.Input.Key.Left))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.SteeringState.Left);
            }
            else if (Keyboard.IsKeyDown(Avalonia.Input.Key.Right))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.SteeringState.Right);
            }
            else
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.SteeringState.Center);
            }

            if (Keyboard.IsKeyDown(Avalonia.Input.Key.PageUp))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.ShiftStates.ShiftStateNext);
                Keyboard.Keys.Remove(Avalonia.Input.Key.PageUp);
            }
            else if (Keyboard.IsKeyDown(Avalonia.Input.Key.PageDown))
            {
                this.messenger.SendMessageToPowertrain(Models.PowerTrain.ShiftStates.ShiftStatePrew);
            }
        }
    }
}

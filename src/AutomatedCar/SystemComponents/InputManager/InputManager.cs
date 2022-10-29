// <copyright file="InputManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.InputManager
{
    using AutomatedCar.Models.PowerTrain.Packets;

    /// <summary>
    /// Input manager.
    /// </summary>
    internal class InputManager : SystemComponent
    {
        /// <summary>
        /// Messenger object reference.
        /// </summary>
#pragma warning disable SA1401 // Fields should be private
        protected readonly IMessenger messenger;
#pragma warning restore SA1401 // Fields should be private

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
        /// Gets a value indicating whether gets speed overrided.
        /// </summary>
        protected bool IsSpeedOverrided { get; private set; }

        /// <summary>
        /// Gets a value indicating whether gets steering overrided.
        /// </summary>
        protected bool IsSteeringOverrided { get; private set; }

        /// <summary>
        /// Gets a value indicating whether gets gearshift overrided.
        /// </summary>
        protected bool IsGearshiftOverrided { get; private set; }

        /// <summary>
        /// Do process.
        /// </summary>
        public override void Process()
        {
            this.IsGearshiftOverrided = false;
            this.IsSteeringOverrided = false;
            this.IsSpeedOverrided = false;

            // This if can be simplified, if packet can use pedalstate insted of new Speedstate.
            if (this.virtualFunctionBus.PowerTrainPacketForSpeed is not null)
            {
                this.IsSpeedOverrided = true;
                if (this.virtualFunctionBus.PowerTrainPacketForSpeed.State == SpeedStates.SpeedUp)
                {
                    this.messenger.SendMessageToPowertrain(Models.PowerTrain.PedalStates.Throtle);
                }
                else if (this.virtualFunctionBus.PowerTrainPacketForSpeed.State == SpeedStates.Break)
                {
                    this.messenger.SendMessageToPowertrain(Models.PowerTrain.PedalStates.Break);
                }
            }

            if (this.virtualFunctionBus.PowerTrainPacketForSteering is not null)
            {
                this.IsSteeringOverrided = true;
                this.messenger.SendMessageToPowertrain(this.virtualFunctionBus.PowerTrainPacketForSteering.Command);
            }

            if (this.virtualFunctionBus.PowerTrainPacketForSpeed is not null)
            {
                this.IsGearshiftOverrided = true;
                this.messenger.SendMessageToPowertrain(this.virtualFunctionBus.PowerTrainPacketForGearshift.GearshiftCommand);
            }
        }
    }
}

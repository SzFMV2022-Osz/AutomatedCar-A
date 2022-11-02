// <copyright file="PowertrainPacket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Packets
{
    using AutomatedCar.SystemComponents.InputManager.Messenger;
    using AutomatedCar.SystemComponents.Powertrain;
    using ReactiveUI;
    using System.Reflection.Metadata.Ecma335;

    /// <summary>
    /// Packet for the VFB to communicate with other systems.
    /// </summary>
    public class PowertrainPacket : ReactiveObject, IPowertrainPacket
    {
        private float currentThrottleValue;
        private float currentBrakeValue;
        private int currentSpeed;
        private string currentGear;
        private int rpm;
        //private SteeringState steeringState;
        private double rotationAngle;
        private string left;
        private string right;

        /// <summary>
        /// Gets or sets the percentage of the gas pedal.
        /// </summary>
        public float CurrentThrottleValue
        {
            get
            {
                return this.currentThrottleValue;
            }

            set => this.RaiseAndSetIfChanged(ref this.currentThrottleValue, value);
        }

        /// <summary>
        /// Gets or sets the percentage of the brake pedal.
        /// </summary>
        public float CurrentBrakeValue
        {
            get
            {
                return this.currentBrakeValue;
            }

            set => this.RaiseAndSetIfChanged(ref this.currentBrakeValue, value);
        }

        /// <summary>
        /// Gets or sets the current speed of the car in Km/h.
        /// </summary>
        public int CurrentSpeed
        {
            get
            {
                return this.currentSpeed;
            }

            set => this.RaiseAndSetIfChanged(ref this.currentSpeed, value);
        }

        /// <summary>
        /// Gets or sets the current gearbox state. Values: P,R,N,D.
        /// </summary>
        public string CurrentGear
        {
            get => this.currentGear;

            set => this.RaiseAndSetIfChanged(ref this.currentGear, value);
        }

        /// <summary>
        /// Gets or sets the current revolution of the engine.
        /// </summary>
        public int Rpm
        {
            get
            {
                return this.rpm;
            }

            set => this.RaiseAndSetIfChanged(ref this.rpm, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the car is steered to the right or not.
        /// </summary>
        public string Left
        {
            get
            {
                return this.left;
            }

            set => this.RaiseAndSetIfChanged(ref this.left, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the car is steered to the right or not.
        /// </summary>
        public string Right
        {
            get
            {
                return this.right;
            }

            set => this.RaiseAndSetIfChanged(ref this.right, value);
        }

        /// <summary>
        /// Gets or sets the rotation of the steering wheel.
        /// </summary>
        public double RotationAngle
        {
            get
            {
                return this.rotationAngle;
            }

            set => this.RaiseAndSetIfChanged(ref this.rotationAngle, value);
        }
    }
}

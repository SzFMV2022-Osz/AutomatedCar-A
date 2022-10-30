// <copyright file="Powertrain.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Threading.Tasks;
    using AutomatedCar.Models.PowerTrain;
    using AutomatedCar.SystemComponents.InputManager;
    using AutomatedCar.SystemComponents.Packets;

    /// <summary>
    /// Powertrain.
    /// </summary>
    public class Powertrain : SystemComponent
    {
        private IEngine engine;

        private ISteering steering;

        private Action[] tasks = new Action[2];
        private Func<float>[] tasksWithReturns = new Func<float>[3];
        private Task<float> drivingforce;
        private Task shift;

        /// <summary>
        /// Initializes a new instance of the <see cref="Powertrain"/> class.
        /// </summary>
        /// <param name="vfb">Virtual function bus.</param>
        /// <param name="messenger">Imessenger.</param>
        public Powertrain(VirtualFunctionBus vfb, IMessenger messenger)
            : base(vfb)
        {
            this.engine = new Engine(new Gearshift());
            this.tasks[0] = this.engine.StateUp;
            this.tasks[1] = this.engine.StateDown;
            this.tasksWithReturns[0] = this.engine.Accelerate;
            this.tasksWithReturns[1] = this.engine.Breaking;
            this.tasksWithReturns[2] = this.engine.Lift;
            messenger.OnPedalChanged += this.Messenger_OnPedalChanged;
            messenger.OnShiftStateChanged += this.Messenger_OnShiftStateChanged;
        }

        /// <summary>
        /// Process.
        /// </summary>
        public override void Process()
        {
            if (this.shift is not null && !this.shift.IsCompleted)
            {
                this.shift.Wait();
            }

            if (this.drivingforce is not null && !this.drivingforce.IsCompleted)
            {
                this.drivingforce.Wait();
                this.virtualFunctionBus.MoveObject = new MoveObject(new Avalonia.Vector(0, -.1 * this.drivingforce.Result));
                this.virtualFunctionBus.DasboardPacket = new DasboardPacket(this.engine.RPM, this.engine.Speed, this.engine.Throtle, this.engine.Break, this.engine.GearShiftState);
            }

            // Task.WaitAll(this.tasksWithReturns);
        }

        private void Messenger_OnShiftStateChanged(object sender, ShiftStates e)
        {
            if (e == ShiftStates.ShiftStateNext)
            {
                this.shift = new Task(this.tasks[0], TaskCreationOptions.LongRunning);
            }
            else
            {
                this.shift = new Task(this.tasks[1], TaskCreationOptions.LongRunning);
            }

            this.shift.Start();
        }

        private void Messenger_OnPedalChanged(object sender, PedalStates e)
        {
            if (e == PedalStates.Throtle)
            {
                this.drivingforce = new Task<float>(this.tasksWithReturns[0], TaskCreationOptions.LongRunning);
            }
            else if (e == PedalStates.Break)
            {
                this.drivingforce = new Task<float>(this.tasksWithReturns[1], TaskCreationOptions.LongRunning);
            }
            else if (e == PedalStates.None)
            {
                this.drivingforce = new Task<float>(this.tasksWithReturns[2], TaskCreationOptions.LongRunning);
            }

            this.drivingforce.Start();
        }
    }
}

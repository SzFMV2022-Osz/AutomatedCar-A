namespace AutomatedCar.SystemComponents.Powertrain
{
    using System;
    using System.Threading.Tasks;
    using AutomatedCar.SystemComponents.InputManager;

    /// <summary>
    /// Powertrain.
    /// </summary>
    public class Powertrain : SystemComponent
    {
        private IEngine engine;

        private ISteering steering;

        private Task[] tasks = new Task[2];
        private Task<float>[] tasksWithReturns = new Task<float>[3];

        /// <summary>
        /// Initializes a new instance of the <see cref="Powertrain"/> class.
        /// </summary>
        /// <param name="vfb">Virtual function bus.</param>
        /// <param name="messenger">Imessenger.</param>
        public Powertrain(VirtualFunctionBus vfb, IMessenger messenger)
            : base(vfb)
        {
            this.engine = new Engine(new Gearshift());
            this.tasks[0] = new Task(new Action(this.engine.StateUp), TaskCreationOptions.LongRunning);
            this.tasks[1] = new Task(new Action(this.engine.StateDown), TaskCreationOptions.LongRunning);
            this.tasksWithReturns[0] = new Task<float>(new Func<float>(this.engine.Accelerate), TaskCreationOptions.LongRunning);
            this.tasksWithReturns[1] = new Task<float>(new Func<float>(this.engine.Breaking), TaskCreationOptions.LongRunning);
            this.tasksWithReturns[2] = new Task<float>(new Func<float>(this.engine.Lift), TaskCreationOptions.LongRunning);
            messenger.OnPedalChanged += this.Messenger_OnPedalChanged;
            messenger.OnShiftStateChanged += this.Messenger_OnShiftStateChanged;
        }

        /// <summary>
        /// Process.
        /// </summary>
        public override void Process()
        {
            Task.WaitAll(this.tasks);
            Task.WaitAll(this.tasksWithReturns);
        }

        private void Messenger_OnShiftStateChanged(object sender, Models.PowerTrain.ShiftStates e)
        {
            if (e == Models.PowerTrain.ShiftStates.ShiftStateNext)
            {
                this.tasks[0].Start();
            }
            else
            {
                this.tasks[1].Start();
            }
        }

        private void Messenger_OnPedalChanged(object sender, Models.PowerTrain.PedalStates e)
        {
            if (e == Models.PowerTrain.PedalStates.Throtle)
            {
                this.tasksWithReturns[0].Start();
            }
            else if (e == Models.PowerTrain.PedalStates.Break)
            {
                this.tasksWithReturns[1].Start();
            }
            else
            {
                this.tasksWithReturns[2].Start();
            }
        }
    }
}

namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents.Sensors;
    using global::AutomatedCar.SystemComponents.Packets;
    using SystemComponents;
    using SystemComponents.InputManager;
    using SystemComponents.Powertrain;
    using SystemComponents.Dasboard;
    using SystemComponents.Locator;
    using Models.PowerTrain;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        private Sensor radarSensor;
        private Messenger messenger;
        private Powertrain powertrain;
        private static int id = 0;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.Dasboard = new DasboardModel();
            this.radarSensor = new Radar(this.virtualFunctionBus);
            this.messenger = new Messenger();
            if (id == 0)
            {
                new KeyboardInputManager(this.virtualFunctionBus, this.messenger);
                new Dasboard(this.VirtualFunctionBus, this.Dasboard);
                ++id;
            }
            else
            {
                new InputManager(this.virtualFunctionBus, this.messenger);
            }

            this.powertrain = new Powertrain(this.virtualFunctionBus, this.messenger);
            new Locator(this.virtualFunctionBus, this);
            this.virtualFunctionBus.CarCoordinatesPacket = new CarCoordinatesPacket(x, y);
            this.ZIndex = 10;
        }

        public DasboardModel Dasboard { get; private set; }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

        public CarCollisionDetector CarCollisionDetector { get; set; }

        public int Revolution { get; set; }

        public int Velocity { get; set; }

        public PolylineGeometry Geometry { get; set; }

        /// <summary>Starts the automated cor by starting the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Start()
        {
            this.virtualFunctionBus.Start();
        }

        /// <summary>Stops the automated cor by stopping the ticker in the Virtual Function Bus, that cyclically calls the system components.</summary>
        public void Stop()
        {
            this.virtualFunctionBus.Stop();
        }
    }
}
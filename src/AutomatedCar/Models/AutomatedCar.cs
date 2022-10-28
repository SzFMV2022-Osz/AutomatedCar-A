namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents.Sensors;
    using global::AutomatedCar.SystemComponents.Packets;
    using SystemComponents;
    using SystemComponents.InputManager;
    using SystemComponents.Powertrain;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        private Sensor radarSensor;
        private Messenger messenger;
        private Powertrain powertrain;
        static int id = 0;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.radarSensor = new Radar(this.virtualFunctionBus);
            if (id == 0)
            {
                this.messenger = new Messenger();
                new InputManager(this.virtualFunctionBus, this.messenger);
                this.powertrain = new Powertrain(this.virtualFunctionBus, this.messenger);
                ++id;
            }

            this.virtualFunctionBus.CarCoordinatesPacket = new CarCoordinatesPacket(x, y);
            this.ZIndex = 10;
        }

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
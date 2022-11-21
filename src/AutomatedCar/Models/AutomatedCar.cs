namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents;
    using global::AutomatedCar.SystemComponents.Packets;
    using global::AutomatedCar.SystemComponents.Powertrain;
    using global::AutomatedCar.SystemComponents.Sensors;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        private Sensor radarSensor;

        private Sensor cameraSensor;

        private PowertrainManager powertrainManager;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.virtualFunctionBus = new VirtualFunctionBus();
            this.radarSensor = new Radar(this.virtualFunctionBus);
            this.cameraSensor = new Camera(this.virtualFunctionBus);
            this.ZIndex = 10;

            this.powertrainManager = new PowertrainManager(this.virtualFunctionBus);
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
// <copyright file="AutomatedCar.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models
{
    using Avalonia.Media;
    using global::AutomatedCar.SystemComponents;
    using global::AutomatedCar.SystemComponents.CruiseControl;
    using global::AutomatedCar.SystemComponents.InputManager.InputHandler;
    using global::AutomatedCar.SystemComponents.Powertrain;
    using global::AutomatedCar.SystemComponents.Sensors;

    public class AutomatedCar : Car
    {
        private VirtualFunctionBus virtualFunctionBus;

        private Sensor radarSensor;
        
        private Sensor cameraSensor;

        private CarCollisionDetector carCollisionDetector;
        
        private PowertrainManager powertrainManager;

        private InputManager inputManager;

        public CruiseControl cruiseControl;

        public AutomaticEmergencyBreak AEB;

        public AutomatedCar(int x, int y, string filename)
            : base(x, y, filename)
        {
            this.Collideable = true;
            this.WorldObjectType = WorldObjectType.Car;

            this.virtualFunctionBus = new VirtualFunctionBus();
            this.radarSensor = new Radar(this.virtualFunctionBus);
            this.cameraSensor = new Camera(this.virtualFunctionBus);
            this.ZIndex = 10;
            this.inputManager = new InputManager(this.virtualFunctionBus);
            this.powertrainManager = new PowertrainManager(this.virtualFunctionBus);
            this.cruiseControl = new CruiseControl(this.virtualFunctionBus);
            this.AEB = new AutomaticEmergencyBreak(this.virtualFunctionBus);
        }

        public VirtualFunctionBus VirtualFunctionBus { get => this.virtualFunctionBus; }

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
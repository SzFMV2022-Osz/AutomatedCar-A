namespace AutomatedCar.SystemComponents.Dashboard
{
    using AutomatedCar.SystemComponents.Packets;
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DashboardManager : SystemComponent
    {
        private IPowertrainPacket powertrainPacket;

        public event EventHandler PropertyChanged;
        private int rpm;

        public int Rpm
        {
            get { return rpm; }
            set
            {
                rpm = value;
                this.PropertyChanged?.Invoke(this, null);
            }
        }

        public int CurrentSpeed { get; set; }
        public int CurrentBrakeValue { get; set; }
        //public int CurrentThrottleValue { get; set; }
        public double RotationAngle { get; set; }
        public string CurrentGear { get; set; }
        public string Steering { get; set; }

        public DashboardManager(VirtualFunctionBus virtualFunctionBus) : base(virtualFunctionBus)
        {
            this.powertrainPacket = new PowertrainPacket();
            virtualFunctionBus.PowertrainPacket = this.powertrainPacket;
        }

        public override void Process()
        {
            /*Rpm = this.virtualFunctionBus.PowertrainPacket.Rpm;
            CurrentSpeed = this.virtualFunctionBus.PowertrainPacket.CurrentSpeed;
            CurrentBrakeValue = this.virtualFunctionBus.PowertrainPacket.CurrentBrakeValue;
            CurrentThrottleValue = this.virtualFunctionBus.PowertrainPacket.CurrentThrottleValue;
            RotationAngle = this.virtualFunctionBus.PowertrainPacket.RotationAngle;
            CurrentGear = this.virtualFunctionBus.PowertrainPacket.CurrentGear.ToString();
            //Steering = this.virtualFunctionBus.PowertrainPacket.Steering.ToString();*/
        }
    }
}

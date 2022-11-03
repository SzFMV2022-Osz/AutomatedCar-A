namespace AutomatedCar.SystemComponents.Packets
{
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class CarCoordinatesPacket : ReactiveObject, ICarCoordinatesPacket
    {
        private int x;

        private int y;

        private double rotation;

        public int X 
        { 
            get => this.x;
            set => this.RaiseAndSetIfChanged(ref this.x, value);
        }

        public int Y
        {
            get => this.y;
            set => this.RaiseAndSetIfChanged(ref this.y, value);
        }

        public double Rotation
        {
            get => this.rotation;
            set => this.RaiseAndSetIfChanged(ref this.rotation, value);
        }

        public CarCoordinatesPacket(int x, int y, double rotation)
        {
            X = x;
            Y = y;
            this.Rotation = rotation;
        }
    }
}

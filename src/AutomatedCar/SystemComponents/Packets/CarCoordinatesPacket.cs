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

        public CarCoordinatesPacket(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}

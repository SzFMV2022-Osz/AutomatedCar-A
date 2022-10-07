namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ICarCoordinatesPacket
    {
        int X { get; set; }

        int Y { get; set; }
    }
}

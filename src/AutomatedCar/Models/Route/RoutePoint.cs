namespace AutomatedCar.Models.Route
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class RoutePoint
    {
        public int X { get; }

        public int Y { get; }

        public int Speed { get; }

        public RoutePoint(int x, int y, int speed)
        {
            X = x;
            Y = y;
            Speed = speed;
        }
    }
}

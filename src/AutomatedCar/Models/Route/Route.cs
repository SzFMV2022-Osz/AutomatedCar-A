namespace AutomatedCar.Models.Route
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    internal class Route
    {
        private RoutePoint[] routePoints;

        private int currentPoint = 0;

        private Route(RoutePoint[] routePoints, int startFrom)
        {
            this.routePoints = routePoints;
            this.currentPoint = startFrom;
            this.CurrentPoint = this.CurrentPoint = this.routePoints[this.currentPoint];
        }

        public RoutePoint CurrentPoint { get; private set; }

        public static Route CreateFromJson(string filename, int startFrom = 0)
        {
            StreamReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(filename));

            string line;
            List<RoutePoint> routePoints = new List<RoutePoint>();
            while ((line = reader.ReadLine()) != null)
            {
                string[] coordinate = line.Split(';');
                int x = int.Parse(coordinate[0]);
                int y = int.Parse(coordinate[1]);
                int speed = int.Parse(coordinate[2]);

                routePoints.Add(new RoutePoint(x, y, speed));
            }

            return new Route(routePoints.ToArray(), startFrom);
        }

        public RoutePoint NextPoint()
        {
            if (this.currentPoint >= this.routePoints.Length || this.currentPoint < 0)
            {
                this.currentPoint = 0;
            }

            this.CurrentPoint = this.routePoints[this.currentPoint++];
            return this.CurrentPoint;
        }
    }
}

namespace AutomatedCar.SystemComponents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Avalonia;

    public class WorldObjectTracker
    {
        private const int PointsSize = 10;

        private Queue<Tuple<Point, DateTime>> points;

        public Queue<Tuple<Point, DateTime>> Points { get => this.points; set => this.points = value; }

        public WorldObjectTracker()
        {
            this.Points = new Queue<Tuple<Point, DateTime>>();
        }

        /// <summary>
        /// Calculate average speed if enough data exists (>2), otherwise returns -1.
        /// </summary>
        /// <returns>Average speed in pixel/second.</returns>
        private double CalculateAverageSpeed()
        {
            if (this.Points.Count() < 2)
            {
                return -1.0;
            }

            double distance = 0;

            for (int i = 0; i < this.Points.Count() - 1; ++i)
            {
                var point1 = this.Points.ElementAt(i).Item1;
                var point2 = this.Points.ElementAt(i + 1).Item1;
                distance = Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
            }

            double elapsedTime = (this.Points.Last().Item2 - this.Points.First().Item2).TotalSeconds;

            // speed = distance(pixel)/time(second)
            return distance / elapsedTime;
        }


        public Point CalculateAverageSpeedVector()
        {
            if (this.Points.Count() < 2)
            {
                return default(Point);
            }

            Point accumulator = new Point(0, 0);

            for (int i = 0; i < this.Points.Count() - 1; ++i)
            {
                var point1 = this.Points.ElementAt(i).Item1;
                var point2 = this.Points.ElementAt(i + 1).Item1;
                accumulator += point2 - point1;
            }

            // speed = distance(pixel)/time(second)
            return accumulator / this.Points.Count();
        }

        /// <summary>
        /// Adds new location to the radar's memory.
        /// Oldest data will be replaced if it exceeds the limit.
        /// The data won't be saved if it's is already saved or not enough time elapsed.
        /// </summary>
        /// <param name="newPoint">New point.</param>
        /// <param name="newTimestamp">New point's timestamp.</param>
        public void AddPoint(Point newPoint, DateTime newTimestamp)
        {
            foreach (var entry in this.Points)
            {
                var point = entry.Item1;
                var timestamp = entry.Item2;

                if ((point == newPoint && timestamp == newTimestamp) || (newTimestamp - timestamp).TotalSeconds < Helpers.Constants.SecondsBetweenTrack)
                {
                    return;
                }
            }

            this.Points.Enqueue(new Tuple<Point, DateTime>(newPoint, newTimestamp));

            if (this.Points.Count() > PointsSize)
            {
                this.Points.Dequeue();
            }
        }
    }
}

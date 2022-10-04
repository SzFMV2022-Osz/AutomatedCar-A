namespace AutomatedCar.Helpers
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    public static class CollisionDetection
    {
        private enum Orientation
        {
            ClockWise,
            CounterClockWise,
            Collinear
        }

        private static Orientation GetOrientation(Point p, Point q, Point r)
        {
            double slopeCoefficient = ((q.Y - p.Y) * (r.X - q.X)) - ((q.X - p.X) * (r.Y - q.Y));
            return slopeCoefficient switch
            {
                0 => Orientation.Collinear,
                > 0 => Orientation.CounterClockWise,
                _ => Orientation.ClockWise
            };
        }

        private static bool PointOnLine(Point point, Point lineStart, Point lineEnd)
        {
            Point firstHalf = point - lineStart;
            Point secondHalf = lineEnd - point;

            double dotProduct = (firstHalf.X * secondHalf.X) + (firstHalf.Y * secondHalf.Y);
            double squaredLineLength = Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2);

            return dotProduct > 0 && dotProduct <= squaredLineLength;
        }

        private static double DotProduct(Point pointA, Point pointB) => (pointA.X * pointB.X) + (pointA.Y * pointB.Y);

        private static bool LinesIntersect(Point line1Start, Point line1End, Point line2Start, Point line2End)
        {
            // Logic: https://www.geeksforgeeks.org/check-if-two-given-line-segments-intersect/
            Orientation o1 = GetOrientation(line1Start, line1End, line2Start);
            Orientation o2 = GetOrientation(line1Start, line1End, line2End);
            Orientation o3 = GetOrientation(line2Start, line2End, line1Start);
            Orientation o4 = GetOrientation(line2Start, line2End, line1End);

            bool orientationsDiffer = !o1.Equals(o2) && !o3.Equals(o4);

            if (orientationsDiffer)
            {
                return true;
            }

            if (o1.Equals(Orientation.Collinear) && PointOnLine(line2Start, line1Start, line1End))
            {
                return true;
            }

            if (o2.Equals(Orientation.Collinear) && PointOnLine(line2End, line1Start, line1End))
            {
                return true;
            }

            if (o3.Equals(Orientation.Collinear) && PointOnLine(line1Start, line2Start, line2End))
            {
                return true;
            }

            return o4.Equals(Orientation.Collinear) && PointOnLine(line1Start, line2Start, line2End);
        }

        public static bool BoundingBoxesCollide(PolylineGeometry source, PolylineGeometry destination, int treshold)
        {
            int intersectionCounter = 0;
            for (int sourceIdx = 1; sourceIdx < source.Points.Count - 1; sourceIdx++)
            {
                Tuple<Point, Point> sourceLine = new (source.Points[sourceIdx - 1], source.Points[sourceIdx]);
                for (int destIdx = 1; destIdx < destination.Points.Count -1; destIdx++)
                {
                    Tuple<Point, Point> destinationLine =
                        new (destination.Points[destIdx - 1], destination.Points[destIdx]);

                    if (LinesIntersect(sourceLine.Item1, sourceLine.Item2, destinationLine.Item1, destinationLine.Item2))
                    {
                        intersectionCounter++;
                    }
                }
            }

            return intersectionCounter >= treshold;
        }

        /// <summary>
        /// Checks if a 2d point lies in the area specified by the 3 coordinates of the triangle.
        /// Edge cases:
        ///     - Point lies in edge of the triangle -> returns true
        ///     - Point equals one of the points of the triangle -> returns true.
        /// </summary>
        /// <param name="point">The point we want to check.</param>
        /// <param name="triangle">The coordinates of the 3 points of the triangle. Ordering doesn't matter.</param>
        /// <returns>Whether the point lies inside, or at the edge of the triangle.</returns>
        public static bool PointInTriangle(Point point, Tuple<Point, Point, Point> triangle)
        {
            double dotProduct1 = DotProduct(triangle.Item2 - triangle.Item1, point);
            double dotProduct2 = DotProduct(triangle.Item3 - triangle.Item2, point);
            double dotProduct3 = DotProduct(triangle.Item1 - triangle.Item3, point);

            return dotProduct1 >= 0 && dotProduct2 >= 0 && dotProduct3 >= 0;
        }
    }
}
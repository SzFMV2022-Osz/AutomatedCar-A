namespace AutomatedCar.Helpers
{
    using Avalonia;
    using Avalonia.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

            return dotProduct >= 0 && dotProduct <= squaredLineLength;
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

            return o4.Equals(Orientation.Collinear) && PointOnLine(line1End, line2Start, line2End);
        }

        /// <summary>
        /// Checks if two bounding boxes have >= threshold number of line intersections.
        /// Edge cases:
        ///     - Common Points count as 1 intersection.
        ///     - Common Lines count as 1 intersection.
        ///     - If there are 2 common lines with a common point, they are counted as 2 intersections.
        /// </summary>
        /// <param name="source">The bounding box of the source object.</param>
        /// <param name="destination">The bounding box of the destination object.</param>
        /// <param name="threshold">The number of line intersections, above which we identify a collision. Inclusive bound.</param>
        /// <returns>Whether the number of line intersections >= the treshold.</returns>
        public static bool BoundingBoxesCollide(PolylineGeometry source, PolylineGeometry destination, int threshold)
        {
            int intersectionCounter = 0;

            var commonEndpoints = new List<Point>(source.Points.Where(point => destination.Points.Contains(point)));

            // We want to add the first point to the list for easier iteration.
            // Basically representing the last point -> first point line.
            var sourceWithFirstElement = new List<Point>(source.Points.Concat(new List<Point> { source.Points[0] }));
            var destinationWithFirstElement =
                new List<Point>(destination.Points.Concat(new List<Point> { destination.Points[0] }));

            for (int sourceIdx = 1; sourceIdx < sourceWithFirstElement.Count; sourceIdx++)
            {
                Tuple<Point, Point> sourceLine = new (sourceWithFirstElement[sourceIdx - 1], sourceWithFirstElement[sourceIdx]);
                for (int destIdx = 1; destIdx < destinationWithFirstElement.Count; destIdx++)
                {
                    Tuple<Point, Point> destinationLine =
                        new (destinationWithFirstElement[destIdx - 1], destinationWithFirstElement[destIdx]);

                    // Common endpoints will automatically be added to intersectionCounter at the end.
                    // We don't want to double dip.
                    if (LinesIntersect(sourceLine.Item1, sourceLine.Item2, destinationLine.Item1, destinationLine.Item2)
                        && !(commonEndpoints.Contains(destinationLine.Item1) || commonEndpoints.Contains(destinationLine.Item2)))
                    {
                        intersectionCounter++;
                    }
                }
            }

            // How do we know that we have a common line? source[i] = dest[j] and source[i + 1] = dest[i + 1]
            // If we have a common line, we only want to count it as one intersection.
            // So we delete one of it's endpoints. This approach works even if there are 2 common lines coming from the same endpoint,
            // Since by definition we'll delete the common endpoint, thus the third intersecting point won't count as a line ending.
            for (int i = 0; i < sourceWithFirstElement.Count - 1; i++)
            {
                if (!commonEndpoints.Contains(sourceWithFirstElement[i]))
                {
                    continue;
                }

                for (int j = 0; j < destinationWithFirstElement.Count - 1; j++)
                {
                    if (sourceWithFirstElement[i].Equals(destinationWithFirstElement[j])
                        && sourceWithFirstElement[i + 1].Equals(destinationWithFirstElement[j + 1]))
                    {
                        commonEndpoints.Remove(destinationWithFirstElement[j + 1]);
                    }
                }
            }

            intersectionCounter += commonEndpoints.Count;
            return intersectionCounter >= threshold;
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
            Point v1 = new(triangle.Item2.Y - triangle.Item1.Y, triangle.Item1.X - triangle.Item2.X);
            Point v2 = new(triangle.Item3.Y - triangle.Item2.Y, triangle.Item2.X - triangle.Item3.X);
            Point v3 = new(triangle.Item1.Y - triangle.Item3.Y, triangle.Item3.X - triangle.Item1.X);

            Point v1p = new(point.X - triangle.Item1.X, point.Y - triangle.Item1.Y);
            Point v2p= new(point.X - triangle.Item2.X, point.Y - triangle.Item2.Y);
            Point v3p = new(point.X - triangle.Item3.X, point.Y - triangle.Item3.Y);

            double dotProduct1 = DotProduct(v1, v1p);
            double dotProduct2 = DotProduct(v2, v2p);
            double dotProduct3 = DotProduct(v3, v3p);

            return dotProduct1 >= 0 && dotProduct2 >= 0 && dotProduct3 >= 0;
        }
    }
}
namespace AutomatedCar.Helpers
{
    using System;
    using System.Numerics;

    public static class CollisionDetection
    {
        private enum Orientation
        {
            ClockWise,
            CounterClockWise,
            Collinear
        }

        private static Orientation GetOrientation(Vector2 p, Vector2 q, Vector2 r)
        {
            float slopeCoefficient = ((q.Y - p.Y) * (r.X - q.X)) - ((q.X - p.X) * (r.Y - q.Y));
            return slopeCoefficient switch
            {
                0 => Orientation.Collinear,
                > 0 => Orientation.CounterClockWise,
                _ => Orientation.ClockWise
            };
        }

        private static bool PointOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
        {
            Vector2 firstHalf = point - lineStart;
            Vector2 secondHalf = lineEnd - point;

            float dotProduct = (firstHalf.X * secondHalf.X) + (firstHalf.Y * secondHalf.Y);
            float squaredLineLength =
                (float)(Math.Pow(lineEnd.X - lineStart.X, 2) + Math.Pow(lineEnd.Y - lineStart.Y, 2));

            return dotProduct > 0 && dotProduct <= squaredLineLength;
        }

        private static float DotProduct(Vector2 pointA, Vector2 pointB) => (pointA.X * pointB.X) + (pointA.Y * pointB.Y);

        public static bool LinesIntersect(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
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

        /// <summary>
        /// Checks if a 2d point lies in the area specified by the 3 coordinates of the triangle.
        /// Edge cases:
        ///     - Point lies in edge of the triangle -> returns true
        ///     - Point equals one of the points of the triangle -> returns true.
        /// </summary>
        /// <param name="point">The point we want to check.</param>
        /// <param name="triangle">The coordinates of the 3 points of the triangle. Ordering doesn't matter.</param>
        /// <returns>Whether the point lies inside, or at the edge of the triangle.</returns>
        public static bool PointInTriangle(Vector2 point, Tuple<Vector2, Vector2, Vector2> triangle)
        {
            float dotProduct1 = DotProduct(triangle.Item2 - triangle.Item1, point);
            float dotProduct2 = DotProduct(triangle.Item3 - triangle.Item2, point);
            float dotProduct3 = DotProduct(triangle.Item1 - triangle.Item3, point);

            return dotProduct1 >= 0 && dotProduct2 >= 0 && dotProduct3 >= 0;
        }
    }
}
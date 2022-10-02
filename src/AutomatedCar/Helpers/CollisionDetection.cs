namespace AutomatedCar.Helpers
{
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

        private static bool Intersects(Vector2 line1Start, Vector2 line1End, Vector2 line2Start, Vector2 line2End)
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

        public static bool BoundingBoxesIntersect(PolylineGeometry BoundingBox1, PolylineGeometry BoundingBox2)
        {
            // PolylineGeometry contains Points, which define the bounding box with a Point List. So (p1, p2) defines a line, along with (p2, p3) etc...
            return true;
        }
    }
}
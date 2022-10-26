namespace Tests.CollisionDetection
{
    using Avalonia;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using static AutomatedCar.Helpers.CollisionDetection;


    [TestClass]
    public class PointInTriangleTests
    {
        [TestMethod]
        public void No_Common_Point_Returns_False()
        {
            Tuple<Point, Point, Point> triangle = new(new Point(0, 0), new Point(0, 4), new Point(4, 4));
            Point point = new(15, 20);

            bool pointInTriangle = PointInTriangle(point, triangle);
            
            Assert.IsFalse(pointInTriangle);
        }

        [TestMethod]
        public void Point_Inside_Triangle_Is_Detected()
        {
            Tuple<Point, Point, Point> triangle = new(new Point(0, 0), new Point(0, 4), new Point(4, 4));
            Point point = new(1, 1);
            
            bool pointInTriangle = PointInTriangle(point, triangle);

            Assert.IsTrue(pointInTriangle);
        }

        [TestMethod]
        public void Point_On_Line_Is_Detected()
        {
            Tuple<Point, Point, Point> triangle = new(new Point(0, 0), new Point(0, 4), new Point(4, 4));
            Point point = new(0, 2);
            
            bool pointInTriangle = PointInTriangle(point, triangle);

            Assert.IsTrue(pointInTriangle);
        }

        [TestMethod]
        public void Point_On_Point_Is_Detected()
        {
            Tuple<Point, Point, Point> triangle = new(new Point(0, 0), new Point(0, 4), new Point(4, 4));
            Point point = new(0, 0);
            
            bool pointInTriangle = PointInTriangle(point, triangle);

            Assert.IsTrue(pointInTriangle);
        }

        [TestMethod]
        public void Point_MinusCoordinates_IsDetected()
        {
            Tuple<Point, Point, Point> triangle = new(new Point(-4520, -7235), new Point(5480, -7235), new Point(480, 1425));
            Point point = new(365, 1034);
            
            bool pointInTriangle = PointInTriangle(point, triangle);

            Assert.IsTrue(pointInTriangle);
        }
    }
}
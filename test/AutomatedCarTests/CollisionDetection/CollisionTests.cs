namespace Tests.CollisionDetection
{
    using AutomatedCar.Helpers;
    using Avalonia;
    using Avalonia.Media;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using static AutomatedCar.Helpers.CollisionDetection;

    [TestClass]
    public class CollisionTests
    {
        [TestMethod]
        public void Two_Distant_Objects_Dont_Collide()
        {
            List<Point> sourcePoints = new List<Point>(){ new (3, 2), new (4.7, 3.2), new (5.2, 13.4) };
            List<Point> destinationPoints = new List<Point>() { new(15.2, 24), new(33, 72), new(53, 14) };

            PolylineGeometry sourceTriangle = new PolylineGeometry(points: sourcePoints, isFilled: false);
            PolylineGeometry destinationTriangle = new PolylineGeometry(points: destinationPoints, isFilled: false);

            bool objectsCollide = BoundingBoxesCollide(sourceTriangle, destinationTriangle, 1);

            Assert.IsFalse(objectsCollide);
        }

        [TestMethod]
        public void Common_Edges_Count_As_Single_Collision()
        {
            List<Point> sourcePoints = new List<Point>(){ new (3, 2), new (4.7, 3.2), new (5.2, 13.4) };
            List<Point> destinationPoints = new List<Point>() { new(3, 2), new(-33, -72), new(-53, -14) };

            PolylineGeometry sourceTriangle = new PolylineGeometry(points: sourcePoints, isFilled: false);
            PolylineGeometry destinationTriangle = new PolylineGeometry(points: destinationPoints, isFilled: false);

            // We can only specify threshold, but we want to know if they collide on exactly one point.
            // So we test if we have more than one collision
            bool lessThanTwoCollisions = !BoundingBoxesCollide(sourceTriangle, destinationTriangle, 2);
            bool atLeastOneCollision = BoundingBoxesCollide(sourceTriangle, destinationTriangle, 1);

            Assert.IsTrue(lessThanTwoCollisions);
            Assert.IsTrue(atLeastOneCollision);
        }

        [TestMethod]
        public void Common_Line_Counts_As_Single_Collision()
        {
            List<Point> sourcePoints = new List<Point>(){ new (3, 2), new (4.7, 3.2), new (5.2, 13.4) };
            List<Point> destinationPoints = new List<Point>() { new(3, 2), new(4.7, 3.2), new(-53, -14) };

            PolylineGeometry sourceTriangle = new PolylineGeometry(points: sourcePoints, isFilled: false);
            PolylineGeometry destinationTriangle = new PolylineGeometry(points: destinationPoints, isFilled: false);

            bool lessThanTwoCollisions = !BoundingBoxesCollide(sourceTriangle, destinationTriangle, 2);
            bool atLeastOneCollision = BoundingBoxesCollide(sourceTriangle, destinationTriangle, 1);
            
            
            Assert.IsTrue(lessThanTwoCollisions);
            Assert.IsTrue(atLeastOneCollision);
        }

        [TestMethod]
        public void Two_Collisions_Against_Single_Line_Get_Counted_Separately()
        {
            List<Point> sourcePoints = new List<Point>(){ new (0, 0), new (4, 0), new (4, 4), new (0, 4)};
            List<Point> destinationPoints = new List<Point>() { new(0, -1), new(4, -1), new(2, 2) };

            PolylineGeometry sourceRectangle = new PolylineGeometry(points: sourcePoints, isFilled: false);
            PolylineGeometry destinationTriangle = new PolylineGeometry(points: destinationPoints, isFilled: false);

            bool lessThanThreeIntersections = !BoundingBoxesCollide(sourceRectangle, destinationTriangle, 3);
            bool atLeastTwoIntersections = BoundingBoxesCollide(sourceRectangle, destinationTriangle, 2);
            
            
            Assert.IsTrue(lessThanThreeIntersections);
            Assert.IsTrue(atLeastTwoIntersections);
        }

        [TestMethod]
        public void Collision_Detection_Is_Commutative()
        {
            // Code segment taken from Two_Distant_Objects_Dont_Collide
            List<Point> sourcePoints = new List<Point>(){ new (3, 2), new (4.7, 3.2), new (5.2, 13.4) };
            List<Point> destinationPoints = new List<Point>() { new(15.2, 24), new(33, 72), new(53, 14) };

            PolylineGeometry sourceTriangle = new PolylineGeometry(points: sourcePoints, isFilled: false);
            PolylineGeometry destinationTriangle = new PolylineGeometry(points: destinationPoints, isFilled: false);

            bool sourceDoesntCollideWithDestination = BoundingBoxesCollide(sourceTriangle, destinationTriangle, 1);
            bool destinationDoesntCollideWithSource = BoundingBoxesCollide(destinationTriangle, sourceTriangle, 1);
            Assert.AreEqual(sourceDoesntCollideWithDestination, destinationDoesntCollideWithSource);
        }
    }
}
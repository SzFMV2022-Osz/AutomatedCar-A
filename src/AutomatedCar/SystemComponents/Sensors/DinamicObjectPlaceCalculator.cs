// <copyright file="DinamicObjectPlaceCalculator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Sensors
{
    using System;
    using System.Collections.Generic;
    using Avalonia;

    /// <summary>
    /// Calculate dinamic object positions.
    /// </summary>
    internal class DinamicObjectPlaceCalculator : IDinamicObjectPlaceCalculator
    {
        /// <summary>
        /// Calculate the posible points.
        /// </summary>
        /// <param name="a">Fist point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="c">Third point.</param>
        /// <param name="pointCount">Next x point.</param>
        /// <returns>Possible object info.</returns>
        public List<DinamicObjectInformationHolder> CalculatePoints(Point a, Point b, Point c, int pointCount)
        {
            var points = new List<DinamicObjectInformationHolder>();
            var v1 = new Vector(a, b);
            var v2 = new Vector(b, c);

            // var scalar = (v1.X * v2.X) + (v1.Y * v2.Y);
            // var degree = (Math.PI / 180) * (scalar / (v1.Length() * v2.Length())); // a-b vector által bezárt szög
            var degree = v1.DegreeCalculation(v2);
            var tempvecor = v2;
            var tempvecor1 = v2;
            for (int i = 0; i < pointCount; i++)
            {
                tempvecor.Rotate(degree);
                tempvecor.Move(tempvecor1);
                points.Add(new DinamicObjectInformationHolder(tempvecor.B, tempvecor.GetVecotrRotation));
                tempvecor1 = tempvecor;
            }

            return points;
        }

        private class Vector
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Vector"/> class.
            /// </summary>
            /// <param name="a">Start point.</param>
            /// <param name="b">End point.</param>
            public Vector(Point a, Point b)
            {
                this.A = a;
                this.B = b;
            }

            /// <summary>
            /// Gets the start point of the vector.
            /// </summary>
            public Point A { get; private set; }

            /// <summary>
            /// Gets the end point of the vector.
            /// </summary>
            public Point B { get; private set; }

            /// <summary>
            /// Gets the length of the vector.
            /// </summary>
            public double Length
            {
                get { return Math.Sqrt(Math.Pow(this.VectorShortForm.X, 2) + Math.Pow(this.VectorShortForm.Y, 2)); }
            }

            /// <summary>
            /// Gets vector shortform.
            /// </summary>
            public (double X, double Y) VectorShortForm
            {
                get { return (this.B.X - this.A.X, this.B.Y - this.A.Y); }
            }

            /// <summary>
            /// Gets Vector rotation in degree.
            /// </summary>
            public double GetVecotrRotation
            {
                get { return (Math.PI / 180) * Math.Acos((this.VectorShortForm.X * 1) + (this.VectorShortForm.Y * (0 - 0))) / (this.Length * 1); }
            }

            /// <summary>
            /// Rotate the vector with aplha.
            /// </summary>
            /// <param name="alpha">Degree of the rotation.</param>
            public void Rotate(double alpha)
            {
                Point tempB = new Point((int)(this.VectorShortForm.X * Math.Cos(alpha)), (int)(this.VectorShortForm.Y * Math.Sin(alpha)));
                this.B = new Point(tempB.X + this.A.X, tempB.Y + this.A.Y);
            }

            /// <summary>
            /// Calcualte the degree between two vectors.
            /// </summary>
            /// <param name="vector">Other vector.</param>
            /// <returns>Degree between two vectors.</returns>
            public double DegreeCalculation(Vector vector)
            {
                if (this.VectorShortForm.Equals(vector.VectorShortForm))
                {
                    return 0;
                }

                return (Math.PI / 180) * Math.Acos(((this.VectorShortForm.X * vector.VectorShortForm.X) +
                    (this.VectorShortForm.Y * vector.VectorShortForm.Y)) / (this.Length * vector.Length));
            }

            /// <summary>
            /// Move vector with other vector.
            /// </summary>
            /// <param name="vector">Other vector.</param>
            public void Move(Vector vector)
            {
                this.A = vector.B;
                this.B = new Point(this.B.X + vector.VectorShortForm.X, this.B.Y + vector.VectorShortForm.Y);
            }
        }
    }
}

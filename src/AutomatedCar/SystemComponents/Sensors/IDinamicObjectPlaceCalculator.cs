// <copyright file="IDinamicObjectPlaceCalculator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Sensors
{
    using System.Collections.Generic;
    using System.Drawing;

    /// <summary>
    /// Calculate dinamic object positions.
    /// </summary>
    internal interface IDinamicObjectPlaceCalculator
    {
        /// <summary>
        /// Calculate the posible points.
        /// </summary>
        /// <param name="a">Fist point.</param>
        /// <param name="b">Second point.</param>
        /// <param name="c">Third point.</param>
        /// <param name="pointCount">Next x point.</param>
        /// <returns>Possible points.</returns>
        public List<Point> CalculatePoints(Point a, Point b, Point c, int pointCount);
    }
}

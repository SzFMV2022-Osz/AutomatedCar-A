namespace AutomatedCar.Models
{
    using System;
    using Avalonia;

    /// <summary>
    /// Relative sensor triangle coordinates.
    /// </summary>
    public struct SensorVision
    {
        /// <summary>
        /// Gets or sets relative left coordinate.
        /// </summary>
        public Point Left { get; set; }

        /// <summary>
        /// Gets or sets relative right coordinate.
        /// </summary>
        public Point Right { get; set; }

        /// <summary>
        /// Gets or sets relative sensor coordinate.
        /// </summary>
        public Point SensorPos { get; set; }

        /// <summary>
        /// Calculate relative Sensor vision points.
        /// </summary>
        /// <param name="dist">Distance in meter.</param>
        /// <param name="deg">Angle in degree.</param>
        /// <param name="sensorPos">Relative sensor position on car.</param>
        /// <returns>Calculated positions.</returns>
        public static SensorVision CalculateVision(int dist, double deg, Point sensorPos)
        {
            var rad = Math.PI * ((deg / 2) / 180);
            var gameDist = 50 * dist;

            var x = (int)(gameDist * Math.Tan(rad));
            var y = gameDist;

            return new SensorVision
            {
                Left = new Point(-x, -y),
                Right = new Point(x, -y),
                SensorPos = new Point(sensorPos.X, sensorPos.Y),
            };
        }
    }
}

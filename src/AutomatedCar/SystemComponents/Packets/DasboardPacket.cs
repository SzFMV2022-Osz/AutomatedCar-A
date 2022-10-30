namespace AutomatedCar.SystemComponents.Packets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class DasboardPacket : IDasboardPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DasboardPacket"/> class.
        /// </summary>
        /// <param name="rpm">RPM.</param>
        /// <param name="speed">Speed.</param>
        /// <param name="throtle">Throtle pedal.</param>
        /// <param name="break">Breakpedal.</param>
        /// <param name="shiftState">Shiftstate.</param>
        public DasboardPacket(float rpm, float speed, float throtle, float @break, string shiftState)
        {
            this.RPM = rpm;
            this.Speed = speed;
            this.Throtle = throtle;
            this.Break = @break;
            this.ShiftState = shiftState;
        }

        /// <summary>
        /// gets rpm of the engine.
        /// </summary>
        public float RPM { get; private set; }

        /// <summary>
        /// Gets Speed of the veicle.
        /// </summary>
        public float Speed { get; private set; }

        /// <summary>
        /// Gets throtle pedal %.
        /// </summary>
        public float Throtle { get; private set; }

        /// <summary>
        /// Gets break pedal %.
        /// </summary>
        public float Break { get; private set; }

        /// <summary>
        /// Gets the gearshift state.
        /// </summary>
        public string ShiftState { get; private set; }
    }
}

using System;

namespace AutomatedCar.Models.NPC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Avalonia;
    using global::AutomatedCar.SystemComponents;
    using Route;

    /// <summary>
    /// Represents the NPC car.
    /// </summary>
    internal class NpcPerson : NPC, INPC
    {
        private MockedRoute[] routes;
        private int index;
        private double betterX;
        private double betterY;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcPerson"/> class.
        /// </summary>
        /// <param name="x">X coordinate of current position.</param>
        /// <param name="y">Y coordinate of current position.</param>
        /// <param name="filename">.</param>
        public NpcPerson(Route route, string filename)
            : base(route, filename, 10, true, WorldObjectType.Car)
        {
            this.MoveComponent = new MoveComponent(this.VirtualFunctionBus, this);
            this.VirtualFunctionBus.RegisterComponent(this.MoveComponent);
        }

        /// <inheritdoc/>
        public MoveComponent MoveComponent { get; set; }

        /// <summary>
        /// Sets the coordinates in double format.
        /// </summary>
        public void SetCoordinates()
        {
            this.betterX = this.X;
            this.betterY = this.Y;
        }

        /// <inheritdoc/>
        public void Move()
        {
            double distanceX = Math.Abs(this.betterX - this.Route.CurrentPoint.X);
            double distanceY = Math.Abs(this.betterY - this.Route.CurrentPoint.Y);
            double distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
            Vector newdirection = this.NewDirection();
            double rotation = (Math.Atan2(newdirection.Y, newdirection.X) * (180 / Math.PI)) + 90;

            if (distanceX == 0 && distanceY == 0)
            {
                this.Route.NextPoint();
                this.Speed = this.Route.CurrentPoint.Speed / 10; // TODO: calculate pixels/meter and convert speed from km/h
            }
            else
            {
                if (distanceX <= this.Speed)
                {
                    this.betterX = this.Route.CurrentPoint.X;
                }

                if (distanceY <= this.Speed)
                {
                    this.betterY = this.Route.CurrentPoint.Y;
                }

                if (this.betterX != this.Route.CurrentPoint.X)
                {
                    this.betterX += newdirection.X * this.Speed;
                    this.Rotation = rotation;
                }

                if (this.betterY != this.Route.CurrentPoint.Y)
                {
                    this.betterY += newdirection.Y * this.Speed;
                    this.Rotation = rotation;
                }

                if (distanceX != 0 && distanceY != 0)
                {
                    this.Rotation = rotation;
                }
            }

            this.X = (int)this.betterX;
            this.Y = (int)this.betterY;
        }

        private Vector NewDirection()
        {
            Vector direction = new Vector(this.Route.CurrentPoint.X - this.betterX, this.Route.CurrentPoint.Y - this.betterY);
            double distance = Math.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
            Vector newdirection = new Vector(direction.X / distance, direction.Y / distance);
            if (distance != 0)
            {
                return newdirection;
            }

            return new Vector(0, 0);
        }

        private class MockedRoute
        {
            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}
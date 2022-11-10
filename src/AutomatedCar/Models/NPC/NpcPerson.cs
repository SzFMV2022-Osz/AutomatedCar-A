namespace AutomatedCar.Models.NPC
{
    using System;
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
        /// Sets the points of the route.
        /// </summary>
        public void SetRoute()
        {
            this.routes = new MockedRoute[10];
            this.routes[0] = new MockedRoute { X = 425, Y = 600 };
            this.routes[1] = new MockedRoute { X = 750, Y = 340 };
            this.routes[2] = new MockedRoute { X = 2100, Y = 340 };
            this.routes[3] = new MockedRoute { X = 2560, Y = 560 };
            this.routes[4] = new MockedRoute { X = 2580, Y = 690 };
            this.routes[5] = new MockedRoute { X = 2675, Y = 1100 };
            this.routes[6] = new MockedRoute { X = 2675, Y = 2320 };
            this.routes[7] = new MockedRoute { X = 1170, Y = 2645 };
            this.routes[8] = new MockedRoute { X = 790, Y = 2645 };
            this.routes[9] = new MockedRoute { X = 225, Y = 1950 };

            this.Speed = 2;
            this.index = 0;
        }

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
            var distanceX = Math.Abs(this.betterX - this.routes[this.index].X);
            var distanceY = Math.Abs(this.betterY - this.routes[this.index].Y);
            var distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
            var newDirection = this.NewDirection();
            var rotation = (Math.Atan2(newDirection.Y, newDirection.X) * (180 / Math.PI)) + 90;

            if (distanceX == 0 && distanceY == 0)
            {
                if (this.index < this.routes.Length - 1)
                {
                    this.index++;
                }
                else
                {
                    this.index = 0;
                }
            }
            else
            {
                if (distanceX <= this.Speed)
                {
                    this.betterX = this.routes[this.index].X;
                }

                if (distanceY <= this.Speed)
                {
                    this.betterY = this.routes[this.index].Y;
                }

                if (this.betterX != this.routes[this.index].X)
                {
                    this.betterX += newDirection.X * this.Speed;
                    this.Rotation = rotation;
                }

                if (this.betterY != this.routes[this.index].Y)
                {
                    this.betterY += newDirection.Y * this.Speed;
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
            var direction = new Vector(this.routes[this.index].X - this.betterX, this.routes[this.index].Y - this.betterY);
            var distance = Math.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
            var newDirection = new Vector(direction.X / distance, direction.Y / distance);

            return distance != 0 ? newDirection : new Vector(0, 0);
        }

        private class MockedRoute
        {
            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}
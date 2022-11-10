// <copyright file="NpcCar.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.NPC
{
    using System;
    using Avalonia;
    using global::AutomatedCar.SystemComponents;
    using Route;

    /// <summary>
    /// Represents the NPC car.
    /// </summary>
    internal class NpcCar : NPC, INPC
    {
        private int index;
        private double betterX;
        private double betterY;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCar"/> class.
        /// </summary>
        /// <param name="filename">.</param>
        public NpcCar(Route route, string filename)
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
            var distanceX = Math.Abs(this.betterX - this.Route.CurrentPoint.X);
            var distanceY = Math.Abs(this.betterY - this.Route.CurrentPoint.Y);
            var distance = Math.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
            var newdirection = this.NewDirection();
            var rotation = (Math.Atan2(newdirection.Y, newdirection.X) * (180 / Math.PI)) + 90;

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

            var carWidth = 100; // TODO: should be calculated somewhere based on the image (filename) or get it as a parameter
            var rotationRadians = rotation * Math.PI / 180;
            var offsetX = Math.Cos(rotationRadians) * (carWidth / 2.0);
            var offsetY = Math.Sin(rotationRadians) * (carWidth / 2.0);

            this.X = (int)(this.betterX - offsetX);
            this.Y = (int)(this.betterY - offsetY);
        }

        private Vector NewDirection()
        {
            var direction = new Vector(this.Route.CurrentPoint.X - this.betterX, this.Route.CurrentPoint.Y - this.betterY);
            var distance = Math.Sqrt((direction.X * direction.X) + (direction.Y * direction.Y));
            var newdirection = new Vector(direction.X / distance, direction.Y / distance);

            return distance != 0 ? newdirection : new Vector(0, 0);
        }

        private class MockedRoute
        {
            public int X { get; set; }

            public int Y { get; set; }
        }
    }
}

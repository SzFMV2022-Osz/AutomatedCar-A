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
        private double betterX;
        private double betterY;

        /// <summary>
        /// Initializes a new instance of the <see cref="NpcCar"/> class.
        /// </summary>
        /// <param name="x">X coordinate of current position.</param>
        /// <param name="y">Y coordinate of current position.</param>
        /// <param name="filename">.</param>
        public NpcCar(Route route, string filename)
            : base(route, filename, 10, true, WorldObjectType.Car)
        {
            this.MoveComponent = new MoveComponent(this.VirtualFunctionBus, this);
            this.VirtualFunctionBus.RegisterComponent(this.MoveComponent);
            this.RotationPoint = new System.Drawing.Point(51, 30);
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
            double distancepertick = (this.Speed * 1000 * 50) / (3600 * 60);

            if (rotation < 0)
            {
                rotation += 360;
            }

            if (rotation > 360)
            {
                rotation -= 360;
            }

            if (distance <= distancepertick)
            {
                this.betterX = this.Route.CurrentPoint.X;
                this.betterY = this.Route.CurrentPoint.Y;
                this.Route.NextPoint();
                this.Speed = this.Route.CurrentPoint.Speed;
            }

            if (this.betterX != this.Route.CurrentPoint.X)
            {
                this.betterX += newdirection.X * distancepertick;
            }

            if (this.betterY != this.Route.CurrentPoint.Y)
            {
                this.betterY += newdirection.Y * distancepertick;
            }

            this.Rotation = rotation;
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
    }
}

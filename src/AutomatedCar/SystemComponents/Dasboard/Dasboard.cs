// <copyright file="Dasboard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.SystemComponents.Dasboard
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Dasboard.
    /// </summary>
    public class Dasboard : SystemComponent
    {
        private Models.PowerTrain.DasboardModel dasboard;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dasboard"/> class.
        /// </summary>
        /// <param name="vfb">Bus.</param>
        /// <param name="dasboard">dasboard.</param>
        public Dasboard(VirtualFunctionBus vfb, Models.PowerTrain.DasboardModel dasboard)
            : base(vfb)
        {
            this.dasboard = dasboard;
        }

        public override void Process()
        {
            if (this.virtualFunctionBus.DasboardPacket is not null)
            {
                this.dasboard.RPM = this.virtualFunctionBus.DasboardPacket.RPM;
                this.dasboard.ThrotlePedal = this.virtualFunctionBus.DasboardPacket.Throtle;
                this.dasboard.BreakPedal = this.virtualFunctionBus.DasboardPacket.Break;
                this.dasboard.Speed = this.virtualFunctionBus.DasboardPacket.Speed;
                this.dasboard.ShiftState = this.virtualFunctionBus.DasboardPacket.ShiftState;
            }
        }
    }
}

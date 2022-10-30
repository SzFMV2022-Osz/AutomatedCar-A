// <copyright file="DasboardModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutomatedCar.Models.PowerTrain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Dasboard data.
    /// </summary>
    public class DasboardModel : INotifyPropertyChanged
    {
        private float rpm;
        private float throtlePedal;
        private float breakPedal;
        private float speed;
        private string shiftstate;

        /// <summary>
        /// Property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public float RPM
        {
            get
            {
                return this.rpm;
            }

            set
            {
                this.rpm = value;
                this.NotifyPropertyChanged();
            }
        }

        public float ThrotlePedal
        {
            get
            {
                return this.throtlePedal;
            }

            set
            {
                this.throtlePedal = value;
                this.NotifyPropertyChanged();
            }
        }

        public float BreakPedal
        {
            get
            {
                return this.breakPedal;
            }

            set
            {
                this.breakPedal = value;
                this.NotifyPropertyChanged();
            }
        }

        public float Speed
        {
            get
            {
                return this.speed;
            }

            set
            {
                this.speed = value;
                this.NotifyPropertyChanged();
            }
        }

        public string ShiftState
        {
            get
            {
                return this.shiftstate;
            }

            set
            {
                this.shiftstate = value;
                this.NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CarViewModel : WorldObjectViewModel
    {
        public AutomatedCar Car { get; set; }
        public Boolean ShouldShowAEBWarning => this.Car.VirtualFunctionBus.PowertrainPacket.CurrentSpeed >= 70;

        public CarViewModel(AutomatedCar car) : base(car)
        {
            this.Car = car;
        }
    }
}

namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;

    public class CarViewModel : WorldObjectViewModel
    {
        public AutomatedCar Car { get; set; }

        public CarViewModel(AutomatedCar car)
            : base(car) => this.Car = car;
    }
}

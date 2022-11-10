namespace AutomatedCar.ViewModels
{
    using AutomatedCar.Models;

    public class PopUpWindowViewModel : ViewModelBase
    {
        public PopUpWindowViewModel(AutomatedCar controlledCar) => this.ControlledCar = new CarViewModel(controlledCar);

        public CarViewModel ControlledCar { get; set; }
    }
}

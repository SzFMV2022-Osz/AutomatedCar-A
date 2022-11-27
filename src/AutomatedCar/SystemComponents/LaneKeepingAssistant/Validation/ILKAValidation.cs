namespace AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILKAValidation
    {
        class Coordinate
        {
            public int X { get; set; }

            public int Y { get; set; }
        }

        bool CanBeTurnedOn(List<Coordinate> objects, int carX, int carY);

        bool MustBeTurnedOff(List<Coordinate> objects, int carX, int carY);
    }
}

namespace AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILKAValidation
    {
        class worldObject
        {
            public string Type { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
        Task<bool> CanBeTurnedOn(IEnumerable<worldObject> worldObjects);
        Task<bool> MustBeTurnedOff(IEnumerable<worldObject> worldObjects);
    }
}

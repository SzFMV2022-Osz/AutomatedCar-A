namespace AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using static AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation.ILKAValidation;
    using System;

    public class LKAValidation : ILKAValidation
    {
        IEnumerable<worldObject> worldObjects;

        public async Task<bool> CanBeTurnedOn(IEnumerable<worldObject> objects)
        {
            int carX = 25, carY = 25, carSpeed = 130;

            worldObjects = worldObjects.Where(x => x.Type == "RoadSide");
            return roadSideIsOK(carX, carY) && speedIsOK(carSpeed);
        }

        public async Task<bool> MustBeTurnedOff(IEnumerable<worldObject> objects)
        {
            if (!await CanBeTurnedOn(objects))
                return false;

            // the detected input from the steering wheel should be catched in the PowertrainManager, thus we don't have to deal with it here.
            // Albeit, I made the curveCheck into a function call, because we might have to check something else as well and its easier to just add
            // function calls to the return value and do the calculations in a separate function.
            return curveIsNotOK(); 
        }

        private bool curveIsNotOK()
        {
            throw new NotImplementedException();
        }

        bool roadSideIsOK(int carX, int carY)
        {
            // option 1: there are x amounts of roadsides:
            bool enoughRoadSides = worldObjects.Count() == 10;

            // option 2: there are roadsides in front of as for x meters away.
            // if we decide to do this, then we'd need the current position of our car. I'm going to use a simple carX, carY here
            // but it should be fed into the function alongside the list of worldObjects.
            bool enoughDistance = false;
            int predefinedDistance = 50; // tbd
            worldObject furthestAway = worldObjects.OrderByDescending(x => x.X + x.Y).First();

            if (furthestAway.X + furthestAway.Y - carX + carY > predefinedDistance)
                enoughDistance = true;

            return enoughRoadSides || enoughDistance;
        }

        bool speedIsOK(int carSpeed)
        {
            // tbd: is our current speed relevant to the lane keeping assistant? I'd say its not, just to shave off some time, but if it is, then:
            int predefinedSpeed = 120;
            return carSpeed < predefinedSpeed;
        }
    }
}

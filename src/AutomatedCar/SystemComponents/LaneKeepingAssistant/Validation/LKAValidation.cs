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
            return roadSideIsOK(carX, carY)/* && speedIsOK(carSpeed)*/;
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
            List<worldObject> objects = worldObjects.ToList();
            objects.OrderBy(x => x.X + x.Y);
            int squaredLikeObjectsCount = 0;
            for (int i = 0; i < objects.Count-1; i++)
            {
                bool isSquaredLikeByX = isSquaredLike(objects[i].X, objects[i + 1].X);
                bool isSquaredLikeByY = isSquaredLike(objects[i].Y, objects[i + 1].Y);
                if (isSquaredLikeByX || isSquaredLikeByY)
                {
                    squaredLikeObjectsCount++;
                }
            }

            if (squaredLikeObjectsCount >= objects.Count * 0.8)
            {
                return true;
            }

            return false;
        }

        bool isSquaredLike(int x1, int x2)
        {
            bool result = false;
            int squaredX1 = Convert.ToInt32(Math.Pow(x1, 2));
            int squaredX2 = Convert.ToInt32(Math.Pow(x2, 2));
            int squaredDifference = Math.Abs(squaredX1 - squaredX2);
            int normalDifference = Math.Abs(x1 - x2);
            int normalDifferenceSquared = Convert.ToInt32(Math.Pow(normalDifference, 2));
            if (Convert.ToInt32(Math.Pow(normalDifferenceSquared, 2)) >= squaredDifference * 0.8)
            {
                result = true;
            }
            return result;
        }

        bool roadSideIsOK(int carX, int carY)
        {
            // option 1: there are x amounts of roadsides:
            bool enoughRoadSides = worldObjects.Count() == 10;

            // option 2: there are roadsides in front of as for x meters away.
            // if we decide to do this, then we'd need the current position of our car. I'm going to use a simple carX, carY here
            // but it should be fed into the function alongside the list of worldObjects.
            //bool enoughDistance = false;
            //int predefinedDistance = 50; // tbd
            //worldObject furthestAway = worldObjects.OrderByDescending(x => x.X + x.Y).First();

            //if (furthestAway.X + furthestAway.Y - carX + carY > predefinedDistance)
            //    enoughDistance = true;

            return enoughRoadSides /*|| enoughDistance*/;
        }

        //bool speedIsOK(int carSpeed)
        //{
        //    // tbd: is our current speed relevant to the lane keeping assistant? I'd say its not, just to shave off some time, but if it is, then:
        //    int predefinedSpeed = 120;
        //    return carSpeed < predefinedSpeed;
        //}
    }
}

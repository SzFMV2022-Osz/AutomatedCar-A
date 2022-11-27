namespace AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static AutomatedCar.SystemComponents.LaneKeepingAssistant.Validation.ILKAValidation;

    public class LKAValidation : ILKAValidation
    {
        private List<Coordinate> roadSides;

        int carX;
        
        int carY;

        public bool CanBeTurnedOn(List<Coordinate> objects, int carX, int carY)
        {
            this.roadSides = objects;
            this.carX = carX;
            this.carY = carY;
            return this.RoadSideIsOK();
        }

        public bool MustBeTurnedOff(List<Coordinate> objects)
        {
            if (!this.CanBeTurnedOn(objects, this.carX, this.carY))
            {
                return false;
            }

            // the detected input from the steering wheel should be catched in the PowertrainManager, thus we don't have to deal with it here.
            // Albeit, I made the curveCheck into a function call, because we might have to check something else as well and its easier to just add
            // function calls to the return value and do the calculations in a separate function.
            return this.CurveIsNotOK();
        }

        private bool CurveIsNotOK()
        {
            //since you put roadSides into the roadSides IEnumerable in order of closest to furthest, we dont need to reorder it
            bool isCurveOk = false;
            bool roadCurves = isTheRoadCurving();

            if (!roadCurves)
            {
                isCurveOk = true;
            }
            else
            {
                isCurveOk = this.CheckCurve();
            }

            return isCurveOk;
        }

        private bool CheckCurve()
        {
            bool isCurveOk = false;
            int i = 0;
            do
            {
                string direction = this.GetCurveDirection();
                Coordinate currentRoadSide = this.roadSides[i];
                Coordinate nextRoadSide = this.roadSides[i + 1];
                switch (direction)
                {
                    case "NW":
                        isCurveOk = currentRoadSide.X < nextRoadSide.Y;
                        break;
                    case "SW":
                        isCurveOk = currentRoadSide.Y < nextRoadSide.X;
                        break;
                    case "SE":
                        isCurveOk = (nextRoadSide.Y - currentRoadSide.Y) < nextRoadSide.X;
                        break;
                    case "NE":
                        isCurveOk = (nextRoadSide.X - currentRoadSide.X) > nextRoadSide.Y;
                        break;
                }

                if (!isCurveOk)
                {
                    return false;
                }

                i++;
            }
            while (isCurveOk && i < this.roadSides.Count - 1);

            return isCurveOk;
        }

        private string GetCurveDirection()
        {
            // curves can go in 4 directions:
            // NW - SW - SE - NE
            // we are going to determine the direction from the first two adjacent coords
            Coordinate firstRoadSide = this.roadSides[0];
            Coordinate secondRoadSide = this.roadSides[1];

            // in theory the first and second roadsides shouldnt be the same on both X and Y coordinates,
            // because we removed the adjacent same coords from the list in our isTheRoadCurving() function
            if (firstRoadSide.X == secondRoadSide.X && firstRoadSide.Y == secondRoadSide.Y)
            {
                throw new ArgumentException("the first and second roadSides shouldnt be the same!");
            }
            string direction = string.Empty;

            if (secondRoadSide.X <= firstRoadSide.X && secondRoadSide.Y <= firstRoadSide.Y)
            {
                direction = "NW";
            }
            else if (secondRoadSide.X <= firstRoadSide.X && secondRoadSide.Y >= firstRoadSide.Y)
            {
                direction = "SW";
            }
            else if (secondRoadSide.X >= firstRoadSide.X && secondRoadSide.Y >= firstRoadSide.Y)
            {
                direction = "SE";
            }
            else if (secondRoadSide.X >= firstRoadSide.X && secondRoadSide.Y <= firstRoadSide.Y)
            {
                direction = "NE";
            }

            return direction;
        }

        private bool isTheRoadCurving()
        {
            bool roadCurves = false;
            for (int i = 0; i < this.roadSides.Count - 1; i++)
            {
                if (this.roadSides[i].X != this.roadSides[i + 1].X &&
                    this.roadSides[i].Y != this.roadSides[i + 1].Y)
                {
                    roadCurves = true;
                }
                else
                {
                    this.roadSides.RemoveAt(i);
                }
            }

            return roadCurves;
        }

        bool RoadSideIsOK()
        {
            // option 1: there are x amounts of roadsides:
            bool enoughRoadSides = this.roadSides.Count() == 10;

            // option 2: there are roadsides in front of us for x meters away.
            // if we decide to do this, then we'd need the current position of our car. I'm going to use a simple carX, carY here
            // but it should be fed into the function alongside the list of worldObjects.
            bool enoughDistance = false;
            int predefinedDistance = 50; // random number
            Coordinate furthestAway = this.roadSides.OrderByDescending(x => x.X + x.Y).First();

            if (furthestAway.X + furthestAway.Y - this.carX + this.carY > predefinedDistance)
            {
                enoughDistance = true;
            }

            return enoughRoadSides || enoughDistance;
        }

    }
}

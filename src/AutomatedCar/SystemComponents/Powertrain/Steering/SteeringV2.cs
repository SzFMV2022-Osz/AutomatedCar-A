namespace AutomatedCar.SystemComponents.Powertrain.Steering
{
    using Avalonia;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SteeringV2
    {
        private int steerAngle;
        private int steeringSpeed = 5;
        private Vector carLocation;
        private double carHeading;
        int wheelBase = 130;
        double dt = 1; //1 / 60;

        public SteeringV2()
        {
            steerAngle = 0;
            carLocation = new Vector();
            carHeading = 0;
        }

        public int SteeringAngle { get { return steerAngle; } set { steerAngle = value; } }

        public double CarHeading { get { return carHeading; } set { carHeading = value; } }

        public Vector CarLocation { get { return carLocation; } set { carLocation = value; } }

        public void SteeringRight()
        {
            if (steerAngle < 60)
            {
                steerAngle += steeringSpeed;
            }
        }

        public void SteeringLeft()
        {
            if (steerAngle > -60)
            {
                steerAngle -= steeringSpeed;
            }
        }

        public void SteeringCenter()
        {
            if (steerAngle > 0)
            {
                steerAngle -= steeringSpeed;
            }

            if (steerAngle < 0)
            {
                steerAngle += steeringSpeed;
            }
        }

        public void SetCarLocation(int x, int y)
        {
            carLocation = new Vector(x, y); //dagadék garbage collector
        }

        public void SetCarHeading(double rotation)
        {
            carHeading = rotation;
        }

        public void DoTheMagic(int carSpeed)
        {
            double normalHeading = ConvertAngleFromDegenerateAvalonia(carHeading);
            double normalHeadingRad = toRadian(normalHeading);
            double steerAngleRad = toRadian(steerAngle);

            double frontWheelX = carLocation.X + wheelBase / 2 * Math.Cos(normalHeadingRad);
            double frontWheelY = carLocation.Y + wheelBase / 2 * Math.Sin(normalHeadingRad);
            double backWheelX = carLocation.X - wheelBase / 2 * Math.Cos(normalHeadingRad);
            double backWheelY = carLocation.Y - wheelBase / 2 * Math.Sin(normalHeadingRad);

            backWheelX += carSpeed * dt * Math.Cos(normalHeadingRad);
            backWheelY += carSpeed * dt * Math.Sin(normalHeadingRad);
            frontWheelX += carSpeed * dt * Math.Cos(normalHeadingRad + steerAngleRad);
            frontWheelY += carSpeed * dt * Math.Sin(normalHeadingRad + steerAngleRad);

            int newCarLocationX = Convert.ToInt32((frontWheelX + backWheelX) / 2);
            int newCarLocationY = Convert.ToInt32((frontWheelY + backWheelY) / 2);

            carLocation = new Vector(newCarLocationX, newCarLocationY);
            normalHeadingRad = Math.Atan2(frontWheelY - backWheelY, frontWheelX - backWheelX);
            carHeading = ConvertAngleToDegenerateAvalonia(toDegree(normalHeadingRad));
        }

        double ConvertAngleFromDegenerateAvalonia(double degree)
        {
            degree = toRadian(degree);
            double x = Math.Cos(degree);
            x = computationErrorValuesToZero(x);
            double y = Math.Sin(degree);
            y = computationErrorValuesToZero(y);

            x = x * -1;
            y *= -1;

            double newRad = Math.Atan2(y, x);
            double newDegree = toDegree(newRad);

            newDegree = normalizeDegree(newDegree);
            newDegree += 90;
            newDegree = normalizeDegree(newDegree);

            return Math.Round(newDegree, 2, MidpointRounding.ToEven);
        }

        double ConvertAngleToDegenerateAvalonia(double degree)
        {
            degree -= 90;
            degree = normalizeDegree(degree);

            degree = toRadian(degree);
            double x = Math.Cos(degree);
            x = computationErrorValuesToZero(x);
            double y = Math.Sin(degree);
            y = computationErrorValuesToZero(y);

            x = x * -1;
            y *= -1;

            double newRad = Math.Atan2(y, x);
            double newDegree = toDegree(newRad);

            newDegree = normalizeDegree(newDegree);
            return Math.Round(newDegree, 2, MidpointRounding.ToEven);
        }

        double computationErrorValuesToZero(double val)
        {
            if (val == 6.123233995736766e-17 || val == -1.8369701987210297e-16 || val == 6.123233995736766e-17)
            {
                return 0;
            }
            return val;
        }

        double normalizeDegree(double val)
        {
            if (val < 0)
            {
                val += 360;
            }
            if (val > 360)
            {
                val -= 360;
            }
            return val;
        }

        double toRadian(double degree)
        {
            return (degree / 180) * (Math.PI);
        }

        double toDegree(double radian)
        {
            return (radian / (Math.PI)) * 180;
        }
    }
}

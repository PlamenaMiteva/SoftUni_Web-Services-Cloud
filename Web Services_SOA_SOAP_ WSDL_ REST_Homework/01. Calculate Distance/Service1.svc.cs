using System;

namespace _01.Calculate_Distance
{
    public class ServiceCalcDistance : IServiceCalcDistance
    {
        public double CalcDistance(int startPointX, int startPointY, int endPointX, int endPointY)
        {
            return Math.Sqrt(Math.Pow(((double)startPointX - endPointX), 2) + Math.Pow((startPointY - endPointY), 2));
        }
       
    }
}

using System;
using _02.Distance_Calculator_Client.ServiceReferenceCalcDistance;

namespace _02.Distance_Calculator_Client
{
    class Program
    {
        static void Main()
        {
            ServiceReferenceCalcDistance.ServiceCalcDistanceClient client = new ServiceCalcDistanceClient();
            var result = client.CalcDistance(2, 4, 8, -3);
            Console.WriteLine(result);
            client.Close();
        }
    }
}

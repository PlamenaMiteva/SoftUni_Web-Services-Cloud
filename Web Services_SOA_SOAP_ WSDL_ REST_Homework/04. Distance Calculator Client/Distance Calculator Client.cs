using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace _04.Distance_Calculator_Client
{
    class Program
    {
        static void Main()
        {
            using (var client= new WebClient())
            {
                var response = client.UploadString("http://localhost:53431/api/Points?x1=2&y1=9&x2=8&y2=-7", "POST", "");
                Console.WriteLine(response);
            }
        }
    }
}

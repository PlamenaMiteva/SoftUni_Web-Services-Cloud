using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace _03.Distance_Calculator_REST.Controllers
{
    public class ValuesController : ApiController
    {
        public IHttpActionResult Distance(int x1, int y1, int x2, int y2)
        {
            return this.Ok(Math.Sqrt(Math.Pow(((double)x1 - x2), 2) + Math.Pow((y1 - y2), 2)));
        }
    }
}

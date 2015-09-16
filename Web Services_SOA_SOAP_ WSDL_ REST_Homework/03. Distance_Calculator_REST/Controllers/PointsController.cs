using System;
using System.Web.Http;

namespace _03.Distance_Calculator_REST.Controllers
{
    public class PointsController : ApiController
    {
        public IHttpActionResult Distance(int startX, int startY, int endX, int endY)
        {
            return this.Ok(Math.Sqrt(Math.Pow(((double)startX - endX), 2) + Math.Pow((startY - endY), 2)));
        }
    }
}

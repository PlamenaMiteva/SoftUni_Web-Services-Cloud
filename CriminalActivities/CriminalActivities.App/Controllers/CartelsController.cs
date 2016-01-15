using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CriminalActivities.App.Models.Binding_Models;
using CriminalActivities.App.Models.View_Models;
using CriminalActivities.Data;
using CriminalActivities.Models;
using CriminalActivities.Models.Enums;
using Microsoft.AspNet.Identity;

namespace CriminalActivities.App.Controllers
{
    public class CartelsController : BaseApiController
    {
        public CartelsController(ICriminalActivitiesData data)
            : base(data)
        {
        }

        public IHttpActionResult GetCartels()
        {
            var cartels = this.Data.Cartels.All()
               .OrderByDescending(c => c.Members.Count)
               .ThenBy(c => c.Name)
               .Select(CartelViewModel.Create);

            return this.Ok(cartels);
        }


        //POST /api/cartels
        [Authorize]
        [HttpPost]
        public IHttpActionResult RegisterNewCartel([FromBody]RegisterCriminalBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var existingCartel = this.Data.Cartels.All().FirstOrDefault(c => c.Name == model.Name);
            if (existingCartel != null)
            {
                return Content(HttpStatusCode.Conflict, "A cartel with the same name already exists");
            }
            var cartel = new Cartel()
            {
                Name = model.Name
            };
            if (model.CriminalIDs.Count != 0)
            {
                foreach (var id in model.CriminalIDs)
                {
                    var criminal = this.Data.Criminals.All().FirstOrDefault(c => c.Id == id);
                    if (criminal == null)
                    {
                        return Content(HttpStatusCode.NotFound,
                            string.Format("Criminal with id #{0} does not exist", id));
                    }
                    if (criminal.CartelId != null)
                    {
                        return Content(HttpStatusCode.Conflict,
                            string.Format("Criminal with id #{0} already has a cartel.", id));
                    }
                    cartel.Members.Add(criminal);
                }
            }
            this.Data.Cartels.Add(cartel);
            this.Data.SaveChanges();

            return this.Ok();
        }

        
    }
}

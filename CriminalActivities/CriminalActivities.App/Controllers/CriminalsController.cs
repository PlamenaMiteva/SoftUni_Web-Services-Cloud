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
    public class CriminalsController : BaseApiController
    {
        public CriminalsController(ICriminalActivitiesData data)
            : base(data)
        {
        }
        //GET /api/criminals?Name={Name}&Alias={Alias}
        public IHttpActionResult GetCriminal([FromUri]CriminalBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var criminal = this.Data.Criminals.All().Where(c => c.FullName == model.Name).Select(CriminalViewModel.Create);
            if (model.Alias!=null)
            {
                criminal = this.Data.Criminals.All().Where(c => c.FullName == model.Name && c.Alias == model.Alias).Select(CriminalViewModel.Create);
            }
            if (!criminal.Any())
            {
                return this.NotFound();
            }
            return this.Ok(criminal);
        }

        //POST /api/criminals
        [Authorize]
        [HttpPost]
        public IHttpActionResult RegisterCriminal([FromBody]CriminalBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var criminal = this.Data.Criminals.All().FirstOrDefault(c => c.FullName == model.Name);
            if (criminal!=null)
            {
                return this.Conflict();
            }
            string loggedUserId = this.User.Identity.GetUserId();
            var newCriminal = new Criminal()
            {
                FullName = model.Name,
                Alias = model.Alias,
                Status = Status.Active,
                CreatorId = loggedUserId
            };
            this.Data.Criminals.Add(newCriminal);
            this.Data.SaveChanges();
            var result = this.Data.Criminals.All()
                .Where(c => c.FullName == model.Name)
                .Select(RegisterdCriminalViewModel.Create);
            return this.CreatedAtRoute(
                 "DefaultApi",
                 new { controller = "criminals", Name = model.Name }, result);
        }

        //POST /api/criminals/{id}/addLocation
        [Authorize]
        [HttpPost]
        [Route("api/criminals/{id}/addLocation")]
        public IHttpActionResult AddLocationToCriminal(int id, [FromBody]LocationBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }
            var criminal = this.Data.Criminals.All().FirstOrDefault(c => c.Id == id);
            if (criminal == null)
            {
                return this.NotFound();
            }
            var city = this.Data.Cities.All().FirstOrDefault(c => c.Name == model.CityName);
            if (city==null)
            {
                city = new City()
                {
                    Name = model.CityName
                };
                this.Data.Cities.Add(city);
                this.Data.SaveChanges();
            }
            var dbCity = this.Data.Cities.All().FirstOrDefault(c => c.Name == model.CityName);
            var location = new Location()
            {
                LastSeen = model.Date,
                City = dbCity,
                Criminal = criminal
            };
            this.Data.Locations.Add(location);
            this.Data.SaveChanges();
            var result = this.Data.Locations.All()
                .Where(l => l.Id == location.Id)
                .Select(LocationViewModel.Create);
            return this.Ok(result);
        }


        //POST /api/criminals/{id}/addActivity
        [Authorize]
        [HttpPost]
        [Route("api/criminals/{id}/addActivity")]
        public IHttpActionResult AddActivityToCriminal(int id, [FromBody]ActivityBindingModel model)
        {
            var criminal = this.Data.Criminals.All().FirstOrDefault(c => c.Id == id);
            var type = this.Data.ActivityTypes.All().FirstOrDefault(a => a.Name == model.Type);
            if (criminal == null || type==null)
            {
                return this.NotFound();
            }
            if (model == null)
            {
                return this.BadRequest("Model cannot be null");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var activity = new Activity()
            {
                Description = model.Description,
                ActiveFrom = model.ActiveFrom,
                ActiveTo = model.ActiveTo,
                ActivityType = type,
                Criminal = criminal
            };
            this.Data.Activities.Add(activity);
            this.Data.SaveChanges();
            var result = this.Data.Activities.All()
                .Where(a => a.Id == activity.Id)
                .Select(ActivityViewModel.Create);
            return this.Ok(result);
        }

        
    }
}

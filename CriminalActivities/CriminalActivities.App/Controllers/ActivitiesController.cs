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

namespace CriminalActivities.App.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        public ActivitiesController(ICriminalActivitiesData data)
            : base(data)
        {
        }
        //PUT /api/activities/{id}
        [Authorize]
        [HttpPut]
        [Route("api/activities/{id}")]
        public IHttpActionResult EditActivity(int id, [FromBody]ActivityBindingModel model)
        {
            var activity = this.Data.Activities.All().FirstOrDefault(a => a.Id == id);
            var type = this.Data.ActivityTypes.All().FirstOrDefault(a => a.Name == model.Type);
            if (activity == null || type == null)
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
            activity.Description = model.Description;
            activity.ActiveFrom = model.ActiveFrom;
            activity.ActivityType = type;
            if (model.ActiveTo!=null)
            {
                activity.ActiveTo = model.ActiveTo;
            }
            this.Data.SaveChanges();
            var result = this.Data.Activities.All()
                .Where(a => a.Id == id)
                .Select(ActivityViewModel.Create);
            return this.Ok(result);
        }


        //DELETE /api/activities/{id}
        [Authorize]
        [HttpDelete]
        //[Route("api/activities/{id}")]
        public IHttpActionResult DeleteActivity(int id)
        {
            var activity = this.Data.Activities.All().FirstOrDefault(a => a.Id == id);
            if (activity == null)
            {
                return this.NotFound();
            }
            this.Data.Activities.Delete(activity);
            this.Data.SaveChanges();
            return this.Ok(
                   new
                   {
                       Message = "Activity #" + activity.Id + " deleted."
                   });
        }


        //GET 
        [HttpGet]
        public IHttpActionResult GetActivities([FromUri]SearchActivityBindingModel model)
        {
            var activities = this.Data.Activities.All().AsQueryable();
            if (model.ActivityType!=null)
            {
                var type = this.Data.ActivityTypes.All().FirstOrDefault(a => a.Name == model.ActivityType);
                if (type!=null)
                {
                    activities = activities.Where(a => a.ActivityTypeId == type.Id);
                }
            }
            if (model.CriminalName != null)
            {
                var criminal = this.Data.Criminals.All().FirstOrDefault(c => c.FullName == model.CriminalName);
                if (criminal != null)
                {
                    activities = activities.Where(a => a.CriminalId == criminal.Id);
                }
            }
            return this.Ok(activities.Select(ActivityViewModel.Create));
        }

       
    }
}

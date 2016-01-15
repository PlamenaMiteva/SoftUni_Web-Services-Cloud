using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CriminalActivities.Models;

namespace CriminalActivities.App.Models.View_Models
{
    public class CartelViewModel
    {
        public string Name { get; set; }

        public IEnumerable<CriminalViewModel> Criminals { get; set; }

        public static Expression<Func<Cartel, CartelViewModel>> Create
        {
            get
            {
                return c => new CartelViewModel()
                {
                    Name = c.Name,
                    Criminals = c.Members.Select(m=> new CriminalViewModel()
                    {
                        Name = m.FullName,
                        Alias = m.Alias,
                        BirthDate = m.BirthDate,
                        Status = m.Status
                    })
                };
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using CriminalActivities.Models;
using CriminalActivities.Models.Enums;

namespace CriminalActivities.App.Models.View_Models
{
    public class RegisterdCriminalViewModel
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public Status Status { get; set; }

        public string RegisteredBy { get; set; }

        public static Expression<Func<Criminal, RegisterdCriminalViewModel>> Create
        {
            get
            {
                return c => new RegisterdCriminalViewModel()
                {
                    Name = c.FullName,
                    Alias = c.Alias,
                    Status = c.Status,
                    RegisteredBy = c.Creator.UserName
                };
            }
        }
    }
}
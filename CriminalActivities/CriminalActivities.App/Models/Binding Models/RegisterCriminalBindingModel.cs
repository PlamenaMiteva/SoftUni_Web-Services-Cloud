using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CriminalActivities.App.Models.Binding_Models
{
    public class RegisterCriminalBindingModel
    {
        [Required]
        public string Name { get; set; }

        public List<int> CriminalIDs { get; set; }
    }
}
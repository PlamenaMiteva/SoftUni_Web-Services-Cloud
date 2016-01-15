using System;
using System.ComponentModel.DataAnnotations;

namespace CriminalActivities.App.Models.Binding_Models
{
    public class ActivityBindingModel
    {
        [Required]
        public string Description { get; set; }

        public DateTime ActiveFrom { get; set; }

        public DateTime? ActiveTo{ get; set; }

        [Required]
        public string Type { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace CriminalActivities.App.Models.Binding_Models
{
    public class LocationBindingModel
    {
        [Required]
        public string CityName { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
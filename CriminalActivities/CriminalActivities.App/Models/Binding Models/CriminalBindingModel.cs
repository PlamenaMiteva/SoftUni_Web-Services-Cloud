using System.ComponentModel.DataAnnotations;

namespace CriminalActivities.App.Models.Binding_Models
{
    public class CriminalBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Alias { get; set; }
    }
}
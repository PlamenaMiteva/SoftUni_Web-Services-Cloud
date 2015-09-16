using System.ComponentModel.DataAnnotations;

namespace BugTracker.RestServices.Models
{
    public class AddBugBindingModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
    }
}
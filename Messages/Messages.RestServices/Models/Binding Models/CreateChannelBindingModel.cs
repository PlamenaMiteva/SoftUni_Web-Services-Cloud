namespace Messages.RestServices.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateChannelBindingModel
    {
        [Required]
        public string Name { get; set; }
    }
}
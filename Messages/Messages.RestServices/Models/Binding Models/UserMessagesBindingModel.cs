namespace Messages.RestServices.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserMessagesBindingModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Recipient { get; set; }
    }
}
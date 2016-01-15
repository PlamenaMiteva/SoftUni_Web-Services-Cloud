using System.ComponentModel.DataAnnotations;

namespace Messages.RestServices.Models.Binding_Models
{
    public class ChannelMessagesBindingModel
    {
        [Required]
        public string Text { get; set; }
    }
}
namespace Messages.RestServices.Models.Binding_Models
{
    using System.ComponentModel.DataAnnotations;

    public class LimitBindingModel
    {
        [Range(1,1000)]
        public int Limit { get; set; }
    }
}
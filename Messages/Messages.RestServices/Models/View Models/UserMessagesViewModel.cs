namespace Messages.RestServices.Models.View_Models
{
    using System;
    using System.Linq.Expressions;
    using Messages.Data.Models;

    public class UserMessagesViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime DateSent { get; set; }

        public string Sender { get; set; }

        public static Expression<Func<UserMessage, UserMessagesViewModel>> Create
        {
            get
            {
                return c => new UserMessagesViewModel()
                {
                    Id = c.Id,
                    Text = c.Text,
                    DateSent = c.DateSent,
                    Sender = c.SenderId != null ? c.Sender.UserName : null
                };
            }
        }
    }
}
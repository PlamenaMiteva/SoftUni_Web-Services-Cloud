using System;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public string CommentAuthorId { get; set; }

        public virtual User CommentAuthor { get; set; }

        [Required]
        public int BugId { get; set; }

        [Required]
        public virtual Bug Bug { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}

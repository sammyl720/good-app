using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoodApp.Data.Models
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public Guid CommentId { get; set; }

        [Required]
        public string ReferenceId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Caption { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public CommentType Type { get; set; }

        public virtual ApplicationUser User { get; set; }

        public enum CommentType
        {
            ChallengeComment, DeedComment
        }
    }
}
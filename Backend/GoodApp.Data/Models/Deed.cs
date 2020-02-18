using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GoodApp.Data.Models
{
    public class Deed
    {
        public Deed()
        {
            TaggedUsers = new Collection<ApplicationUser>();
        }

        public Guid DeedId { get; set; }

        [Required]
        public DateTime DeedDate { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Location { get; set; }

        [Required]
        public double Lat { get; set; }

        [Required]
        public double Lon { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        public string Comment { get; set; }

        public Guid ChallengeId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<ApplicationUser> TaggedUsers { get; set; }

        public virtual Challenge Challenge { get; set; }
    }
}
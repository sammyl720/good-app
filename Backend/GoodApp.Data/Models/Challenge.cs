using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoodApp.Data.Models
{
    public class Challenge
    {
        public Challenge()
        {
            Groups = new HashSet<Group>();
        }
        public Guid ChallengeId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        public int FrequencyCount { get; set; }

        // >=1
        [Required]
        public int FrequencyValue { get; set; }

        [Required]
        public string FrequencyType { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public string Picture { get; set; }
        
        [Required]
        [StringLength(128)]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Group> Groups { get; set; }

        public virtual ICollection<Deed> Deeds { get; set; } 
    }
}

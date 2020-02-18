using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodApp.Data.Models
{
    public class Group
    {
        public Group()
        {
            Members = new HashSet<ApplicationUser>();
            Challenges = new Collection<Challenge>();
        }
        public Guid GroupId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public string Picture { get; set; }

        [Required]
        [StringLength(128)]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Challenge> Challenges { get; set; }
        public virtual ICollection<ApplicationUser> Members { get; set; }
    }
}

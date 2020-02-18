using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            CreatedChallenges = new HashSet<Challenge>();
            CreatedGroups = new HashSet<Group>();
            JoinedGroups = new HashSet<Group>();
            MyDeeds = new HashSet<Deed>();
            TagDeeds = new HashSet<Deed>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhotoPath { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime LastLoginDate { get; set; }

        [Required]
        public string Status { get; set; }

        public virtual ICollection<Challenge> CreatedChallenges { get;set; }

        public virtual ICollection<Group> CreatedGroups { get; set; }

        public virtual ICollection<Group> JoinedGroups { get; set; }

        public virtual ICollection<Deed> MyDeeds { get; set; }

        public virtual ICollection<Deed> TagDeeds { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace GoodApp.Data.Views
{
    public class GroupView
    {
        public Guid GroupId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Group Code")]
        public string Code { get; set; }

        [Display(Name = "Creator")]
        public string FullName {
            get { return CreatorFirstName + " " + CreatorLastName; }
        }
        
        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "N/A")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Creator Id")]
        public string CreatorId { get; set; }

        [Display(Name = "Creator User Name")]
        public string CreatorUserName { get; set; }

        [Display(Name = "Creator FirstName")]
        public string CreatorFirstName { get; set; }

        [Display(Name = "Creator LastName")]
        public string CreatorLastName { get; set; }

        [Display(Name = "# Members")]
        public int MembersCount { get; set; }

        [Display(Name = "# Challenges")]
        public int ChallengesCount { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }
    }
}

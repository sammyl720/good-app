using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace GoodApp.Data.Views
{
    public class UserView
    {
        public string Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName {
            get { return FirstName + " " + LastName; }
        }

        [Display(Name = "Picture")]
        public string PhotoPath { get; set; }

        [Display(Name = "Create Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "N/A")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Last Login Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "N/A")]
        public DateTime LastLoginDate { get; set; }

        public string Status { get; set; }

        public string Email { get; set; }

        [Display(Name = "Confirmed?")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "# Joined Group Codes")]
        public int JoinedGroupCount { get; set; }

        [Display(Name = "# Deeds")]
        public int DeedCount { get; set; }

        [Display(Name = "# Valid Deeds")]
        public int ValidDeedCount { get; set; }

        [Display(Name = "# Joined Challenges")]
        public int JoinedChallengeCount { get; set; }

        [Display(Name = "# Recipients")]
        public int TagUserCount { get; set; }

        [Display(Name = "# Total Deeds")]
        public int TotalDeedCount { get; set; }

        [Display(Name = "# Network Deeds")]
        public int NetworkDeedCount { get; set; }
    }
}

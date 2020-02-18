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
    public class DeedView
    {
        public Guid DeedId { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        [Display(Name = "Who?")]
        public string FullName {
            get { return CreatorFirstName + " " + CreatorLastName; }
        }
        [Display(Name = "Deed Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "N/A")]
        public DateTime DeedDate { get; set; }

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

        public Guid ChallengeId { get; set; }

        [Display(Name = "Challenge Name")]
        public string ChallengeName { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", NullDisplayText = "N/A")]
        public DateTime DueDate { get; set; }

        [Display(Name = "# Recipients")]
        public int TagUserCount { get; set; }

        [Display(Name = "Valid?")]
        public bool IsValid { get; set; }

        public string TagUserFirstName { get; set; }

        public string TagUserLastName { get; set; }

        public string TagUserName { get; set; }

        public string TagUserId { get; set; }

        [Display(Name = "Recipient")]
        public string TagUserFullName
        {
            get { return TagUserFirstName + " " + TagUserLastName; }
        }
    }
}

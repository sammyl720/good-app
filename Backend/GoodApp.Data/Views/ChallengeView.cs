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
    public class ChallengeView
    {
        public Guid ChallengeId { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Count")]
        public int Count { get; set; }

        public string Description { get; set; }

        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}",NullDisplayText = "N/A")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Order")]
        public int Order { get; set; }

        [Display(Name = "Frequency Value")]
        public int FrequencyValue { get; set; }

        [Display(Name = "Frequency Count")]
        public int FrequencyCount { get; set; }

        [Display(Name = "Frequency Type")]
        public string FrequencyType { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

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

        [Display(Name = "# Group Codes")]
        public int ChallengeGroupCount { get; set; }

        [Display(Name = "# Deeds")]
        public int DeedCount { get; set; }

        [Display(Name = "# Valid Deeds")]
        public int ValidDeedCount { get; set; }

        [Display(Name = "# Challenge Users")]
        public int ChallengeUserCount { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }
    }
}

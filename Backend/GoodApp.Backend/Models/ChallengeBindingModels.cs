using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GoodApp.Data;
using GoodApp.Data.Models;

namespace GoodApp.Backend.Models
{
    public class EditChallengeBindingModel
    {
        public Guid? ChallengeId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Type")]
        public Enums.ChallengeType Type { get; set; }

        [Required]
        [Display(Name = "Count")]
        public int Count { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode =true, NullDisplayText = "N/A")]
        public DateTime DueDate { get; set; }

        //[Display(Name = "Order")]
        //public string Order { get; set; }

        //[Required]
        //[Display(Name = "Frequency Value")]
        //public int FrequencyValue { get; set; }

        //[Required]
        //[Display(Name = "Frequency Count")]
        //public int FrequencyCount { get; set; }

        //[Required]
        //[Display(Name = "Frequency Type")]
        //public Enums.FrequencyType FrequencyType { get; set; }

        [Required]
        [Display(Name = "Stauts")]
        public Enums.ChallengeStatus Status { get; set; }

        [Display(Name = "Group Code")]
        public string[] GroupIds { get; set; }

        [Display(Name = "Picture")]
        public HttpPostedFileBase Picture { get; set; }

        public string PictureUrl { get; set; }
    }
}
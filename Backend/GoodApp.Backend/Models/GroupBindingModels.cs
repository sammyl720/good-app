using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GoodApp.Backend.Models
{
    public class EditGroupBindingModel
    {
        public Guid? GroupId { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Group Code")]
        public string Code { get; set; }

        [Display(Name = "Picture")]
        public HttpPostedFileBase Picture { get; set; }

        public string PictureUrl { get; set; }
    }
}
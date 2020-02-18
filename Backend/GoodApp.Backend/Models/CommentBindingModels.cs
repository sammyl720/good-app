using System.ComponentModel.DataAnnotations;

namespace GoodApp.Backend.Models
{
    public class CommentBindingModel
    {
        [Required]
        public string Caption { get; set; }
    }
}
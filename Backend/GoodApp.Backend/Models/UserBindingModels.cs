using System.ComponentModel.DataAnnotations;
using TechwuliArsenal.WebApi.MultipartDataMediaFormatter.Infrastructure;

namespace GoodApp.Backend.Models
{
    public class ChangePhotoBindingModel
    {

        [Required]
        [Display(Name = "Photo")]
        public HttpFile Photo { get; set; }
    }
}

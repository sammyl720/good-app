using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodApp.Data.Models
{
    public class Application
    {
        public Guid ApplicationId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string AllowedOrigin { get; set; }
    }
}

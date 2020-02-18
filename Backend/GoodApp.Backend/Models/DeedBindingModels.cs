using System;
using System.ComponentModel.DataAnnotations;

namespace GoodApp.Backend.Models
{
    public class DeedCreateBindingModel
    {
        [Required]
        public DateTime DeedTime { get; set; }

        [Required]
        public string Location { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public string Comment { get; set; }

        public int? Rating { get; set; }

        public string[] TaggedUserIds { get; set; }
    }
}

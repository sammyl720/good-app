using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodApp.Data.Models
{
    public class ErrorMessage
    {
        public Guid ErrorMessageId { get; set; }

        public string Request { get; set; }

        public string Exception { get; set; }

        public DateTime EventDate { get; set; }
    }
}

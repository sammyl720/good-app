using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodApp.Data.Models
{
    public class EmailTemplate
    {
        public Guid EmailTemplateId { get; set; }

        public string FromEmail { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data.Models;

namespace GoodApp.Data.Repositories
{
    public class EmailTemplateRepository : TableRepository<EmailTemplate>
    {
        public EmailTemplateRepository(ApplicationDbContext context)
            : base(context)
        {
            
        }
    }
}

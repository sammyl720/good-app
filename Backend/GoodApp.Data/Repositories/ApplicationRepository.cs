using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data.Models;

namespace GoodApp.Data.Repositories
{
    public class ApplicationRepository : TableRepository<Application>
    {
        public ApplicationRepository(ApplicationDbContext context):base(context)
        {
            
        }
    }
}

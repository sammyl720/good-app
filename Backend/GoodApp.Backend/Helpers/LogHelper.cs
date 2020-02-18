using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data;
using GoodApp.Data.Models;
using GoodApp.Data.Repositories;

namespace GoodApp.Backend.Helpers
{
    public class LogHelper
    {
        public static void Log(string request, string exception)
        {
            RepositoryProvider repositryProvider = RepositoryProvider.Create();
            repositryProvider.Get<ErrorMessageRepository>().Insert(new ErrorMessage()
            {
                ErrorMessageId = Guid.NewGuid(),
                EventDate = DateTime.UtcNow,
                Request = request,
                Exception = exception
            });
            repositryProvider.Save();
        }
    }
}

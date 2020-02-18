using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data;
using Microsoft.Owin;

namespace GoodApp.Backend.Controllers
{
    public interface IBaseController
    {
        CurrentAccess CurrentAccess { get; set; }

        ApplicationUserManager UserManager { get; set; }

        ApplicationRoleManager RoleManager { get; set; }

        RepositoryProvider RepositoryProvider { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using GoodApp.Backend.Controllers;
using GoodApp.Data;
using Microsoft.AspNet.Identity.Owin;

namespace GoodApp.Backend.Api.Controllers
{
    public class BaseApiController : ApiController, IBaseController
    {
        public CurrentAccess CurrentAccess { get; set; }

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                if (Request == null)
                {
                    return null;
                }

                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set { _userManager = value; }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().Get<ApplicationRoleManager>();
            }
            set
            {
                _roleManager = value;
            }
        }

        public RepositoryProvider RepositoryProvider
        {
            get
            {
                return Request.GetOwinContext().Get<RepositoryProvider>();
            }
        }
    }
}

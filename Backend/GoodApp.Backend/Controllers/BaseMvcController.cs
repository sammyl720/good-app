using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodApp.Data;
using Microsoft.AspNet.Identity.Owin;

namespace GoodApp.Backend.Controllers
{
    public class BaseMvcController : Controller, IBaseController
    {

        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? Request.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set { _signInManager = value; }
        }

        public CurrentAccess CurrentAccess { get; set; }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
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

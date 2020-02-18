using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Backend.Controllers;
using GoodApp.Data;
using GoodApp.Data.Models;
using Microsoft.AspNet.Identity;
using GoodApp.Backend.Api.Controllers;

namespace GoodApp.Backend.Helpers
{
    public class ControllerHelper
    {
        public static void InitCurrentAccess(IBaseController controller)
        {
            CurrentAccess currentAccess = controller.CurrentAccess?? new CurrentAccess();
            IPrincipal user = null;
            var mvcController = controller as BaseMvcController;
            if (mvcController != null)
            {
                user = mvcController.User;
            }
            else
            {
                var apiController = controller as BaseApiController;
                if (apiController != null)
                {
                    user = apiController.User;
                }
            }
            if (user != null && user.Identity.IsAuthenticated)
            {
                var appUser = controller.UserManager.FindById(user.Identity.GetUserId());
                if (appUser == null)
                {
                    var claim = user.Identity as ClaimsIdentity;
                    if (claim != null) appUser = controller.UserManager.FindByName(claim.NameClaimType);
                }
                if (appUser != null)
                {
                    currentAccess.UserId = appUser.Id;
                    currentAccess.UserName = appUser.UserName;
                    var roles = controller.UserManager.GetRoles(appUser.Id);
                    currentAccess.RoleName = roles == null ? string.Empty : string.Join(",", roles);
                }
            }
            if (controller.CurrentAccess == null)
            {
                controller.CurrentAccess = currentAccess;
            }
        }
    }
}

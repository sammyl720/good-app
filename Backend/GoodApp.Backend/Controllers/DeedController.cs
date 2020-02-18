using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using GoodApp.Backend.Helpers;
using GoodApp.Backend.Models;
using GoodApp.Data;
using GoodApp.Data.Repositories;
using GoodApp.Data.Views;
using Microsoft.AspNet.Identity;
using GoodApp.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Backend.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DeedController : BaseMvcController
    {
        private readonly string[] _columns = new[] { "ChallengeName", "DeedDate", "Location","Lat","Lon","Rating", "Comment","CreateDate", "CreatorFirstName","IsValid","TagUserCount","TagUserFirstName"};
        public ActionResult Index(string type="ALL", string subType="ALL", string id="ALL",int pageIndex = 0, int pageSize = 0, string search = "", string orderBy = "CreateDate",string challengeId="All",string userId="All", bool isAsc = false)
        {
            int count = 0;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 20 : pageSize;
            orderBy = (string.IsNullOrEmpty(orderBy) || _columns.All(p => !String.Equals(p, orderBy, StringComparison.CurrentCultureIgnoreCase))) ? "CreateDate" : orderBy;
            challengeId = !string.IsNullOrEmpty(challengeId) && challengeId.ToUpper() != "ALL" ? challengeId : "All";
            userId = !string.IsNullOrEmpty(userId) && userId.ToUpper() != "ALL" ? userId : "All";

            string subTitle = string.Empty;
            string preLink = "/Deed/?type=" + type.ToUpper() + "&subType="+subType.ToUpper()+"&id=" + id;
            IEnumerable<DeedView> deedViews;
            if (type.ToUpper() == "CHALLENGE" && !string.IsNullOrEmpty(id))
            {
                var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Challenge'>Challenges</a>&gt;<a href='/Challenge?type=SINGLE&id=" + challenge.ChallengeId + "'>" + challenge.Name + "</a>"
                    + "&gt;<a href='/Deed?type=CHALLENGE&id=" + challenge.ChallengeId + "'>Deeds</a>&gt;All";
            }
            else if (type.ToUpper() == "USER" && !string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);
                subTitle = "<a href='/User'>Users</a>&gt;<a href='/User?type=SINGLE&id=" + user.Id + "'>" + user.FirstName + " " + user.LastName + "</a>"
                    + "&gt;<a href='/Deed?type=USER&id=" + user.Id + "'>Deeds</a>&gt;" + (subType.ToUpper()=="ALL"?"Personal":subType);
            }
            else if (type.ToUpper() == "SINGLE" && !string.IsNullOrEmpty(id))
            {
                var deed = RepositoryProvider.Get<DeedRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Challenge'>Challenges</a>&gt;" + deed.Challenge.Name;
            }
            deedViews = RepositoryProvider.Get<ViewRepository>().GetDeedViews(ref count, pageIndex, pageSize, search, orderBy, challengeId, userId,type,subType,id, isAsc);
            
            ViewBag.PreLink = preLink;
            ViewBag.SubTitle = subTitle;
            ViewBag.Type = type.ToUpper();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.ItemCount = count;
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.IsAsc = isAsc;
            ViewBag.ChallengeId = challengeId;
            if (challengeId.ToUpper() == "ALL")
            {
                ViewBag.DisplayChallenge = "All Challenges";
            }
            else
            {
                var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(challengeId));
                ViewBag.DisplayChallenge = challenge.Name;
            }
            ViewBag.UserId = userId;
            if (userId.ToUpper() == "ALL")
            {
                ViewBag.DisplayUser = "All Users";
            }
            else
            {
                var user = UserManager.FindById(userId);
                ViewBag.DisplayUser = user.FirstName + " " + user.LastName;
            }
            ViewBag.Challenges = RepositoryProvider.Get<ChallengeRepository>().Get().ToList();
            ViewBag.Users = UserManager.Users.ToList();
            return View(deedViews);
        }
        public ActionResult Delete(Guid id)
        {
            RepositoryProvider.Get<DeedRepository>().Delete(id);
            RepositoryProvider.Save();
            return Redirect(Request.UrlReferrer == null ? "/Deed" : Request.UrlReferrer.AbsoluteUri);
        }
    }
}

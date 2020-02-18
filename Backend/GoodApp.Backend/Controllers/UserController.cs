using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoodApp.Backend.Helpers;
using GoodApp.Backend.Models;
using GoodApp.Data;
using GoodApp.Data.Models;
using GoodApp.Data.Repositories;
using GoodApp.Data.Views;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Backend.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : BaseMvcController
    {
        private readonly string[] _columns = new[]
        {
            "FirstName", "Email", "Status", "CreateDate", "JoinedGroupCount", "DeedCount", "ValidDeedCount",
            "JoinedChallengeCount", "TagUserCount", "NetworkDeedCount"
        };

        public ActionResult Index(string type = "ALL", string id = "ALL", int pageIndex = 0, int pageSize = 0,
            string status = "All", string search = "", string orderBy = "CreateDate", bool isAsc = false)
        {
            int count = 0;
            string subTitle = string.Empty;
            string preLink = "/User/?type=" + type.ToUpper() + "&id=" + id;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 20 : pageSize;
            Enums.UserStatus userStatus;
            status = string.IsNullOrEmpty(status) || !Enum.TryParse(status, out userStatus)
                ? "All"
                : userStatus.ToString();
            orderBy = (string.IsNullOrEmpty(orderBy) ||
                       _columns.All(p => !String.Equals(p, orderBy, StringComparison.CurrentCultureIgnoreCase)))
                ? "CreateDate"
                : orderBy;
            IEnumerable<UserView> userViews;


            if (type.ToUpper() == "CODE" && !string.IsNullOrEmpty(id))
            {
                var group = RepositoryProvider.Get<GroupRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Code'>Group Codes</a>&gt;<a href='/Code?type=SINGLE&id=" + group.GroupId + "'>" +
                           group.Code + "</a>&gt;Members";
            }
            else if (type.ToUpper() == "DEED" && !string.IsNullOrEmpty(id))
            {
                var deed = RepositoryProvider.Get<DeedRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Deed'>Deeds</a>&gt;<a href='/Deed?type=SINGLE&id=" + deed.DeedId + "'>" +
                           deed.Challenge.Name + "</a>&gt;Recipients";
            }
            else if (type.ToUpper() == "CHALLENGE" && !string.IsNullOrEmpty(id))
            {
                var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Challenge'>Challenges</a>&gt;<a href='/Challenge?type=SINGLE&id=" +
                           challenge.ChallengeId + "'>" + challenge.Name + "</a>&gt;Users";
            }
            else if (type.ToUpper() == "USER" && !string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);
                subTitle = "<a href='/User'>Users</a>&gt;<a href='/User?type=SINGLE&id=" + user.Id + "'>" +
                           user.FirstName + " " + user.LastName + "</a>&gt;Recipients";
            }
            else if (type.ToUpper() == "SINGLE" && !string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);
                subTitle = "<a href='/User'>Users</a>&gt;" + user.FirstName + " " + user.LastName;
            }
            userViews = RepositoryProvider.Get<ViewRepository>()
                .GetUserViews(ref count, pageIndex, pageSize, status, search, orderBy, type.ToUpper(), id, isAsc);
            ViewBag.PreLink = preLink;
            ViewBag.SubTitle = subTitle;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.ItemCount = count;
            ViewBag.Status = status;
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.IsAsc = isAsc;
            return View(userViews);
        }

        public ActionResult Create()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/User" : Request.UrlReferrer.AbsoluteUri;
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "FirstName,LastName,Photo,UserName,Password,ConfirmPassword,RoleName,GroupIds,Status")] RegisterUserMvcViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindByName(model.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("", "User Name has been userd.");
                    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                        ? "/User"
                        : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                }
                else
                {
                    var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);

                    Stream s = model.Photo.InputStream;
                    ImageHelper.Resize(s, s, 300, ImageFormat.Jpeg);
                    var photoUrl =
                        await
                            azureStorageHelper.SaveFileStream(s, Guid.NewGuid() + ".jpg",
                                AzureStorageHelper.FileUsage.UserPhotos);
                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhotoPath = photoUrl,
                        CreateDate = DateTime.UtcNow,
                        LastLoginDate = DateTime.UtcNow,
                        Status = model.Status.ToString()
                    };
                    var role = RoleManager.FindByName(model.RoleName.ToString());
                    user.Roles.Add(new IdentityUserRole()
                    {
                        UserId = user.Id,
                        RoleId = role.Id
                    });
                    if (model.GroupIds != null && model.GroupIds.Length > 0)
                    {
                        foreach (var gId in model.GroupIds)
                        {
                            Guid id = Guid.Parse(gId);
                            var group = RepositoryProvider.Get<GroupRepository>()
                                .FirstOrDefault(p => p.GroupId == id);
                            if (group != null)
                            {
                                user.JoinedGroups.Add(group);
                            }
                        }
                    }
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        await azureStorageHelper.DeleteFile(photoUrl, AzureStorageHelper.FileUsage.UserPhotos);
                        AddErrors(result);
                        ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/User"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                    }
                    else
                    {
                        return
                            Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                                ? "/User"
                                : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                    }
                }
            }
            ViewBag.ReturnUrl = Request.QueryString["returnUrl"];
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/User" : Request.UrlReferrer.AbsoluteUri;
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var editUser = new EditUserMvcViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoPath = user.PhotoPath,

            };
            Enums.UserStatus userStauts;
            editUser.Status = Enum.TryParse(user.Status, out userStauts) ? userStauts : Enums.UserStatus.Inactive;

            var identityUserRole = user.Roles.FirstOrDefault();
            if (identityUserRole != null)
            {
                var role = RoleManager.FindById(identityUserRole.RoleId);
                Enums.RoleType roleType;
                editUser.RoleName = Enum.TryParse(role.Name, out roleType) ? roleType : Enums.RoleType.User;
            }
            editUser.GroupIds = user.JoinedGroups.Any()
                ? user.JoinedGroups.Select(p => p.GroupId.ToString()).ToArray()
                : null;
            return View(editUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "UserId,FirstName,LastName,PhotoPath,Photo,Status,UserName,RoleName,GroupIds")] EditUserMvcViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(model.UserId);
                if (model.Photo != null && model.Photo.InputStream.Length > 0)
                {
                    var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                    Stream s = model.Photo.InputStream;
                    ImageHelper.Resize(s, s, 300, ImageFormat.Jpeg);
                    var photoUrl =
                        await
                            azureStorageHelper.SaveFileStream(s, Guid.NewGuid() + ".jpg",
                                AzureStorageHelper.FileUsage.UserPhotos);
                    if (!string.IsNullOrEmpty(user.PhotoPath))
                    {
                        await azureStorageHelper.DeleteFile(user.PhotoPath, AzureStorageHelper.FileUsage.UserPhotos);
                    }
                    user.PhotoPath = photoUrl;
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Status = model.Status.ToString();
                user.Roles.Clear();
                var role = RoleManager.FindByName(model.RoleName.ToString());
                user.Roles.Add(new IdentityUserRole()
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
                if (model.GroupIds != null && model.GroupIds.Length > 0)
                {
                    user.JoinedGroups.Clear();
                    foreach (var gId in model.GroupIds)
                    {
                        Guid id = Guid.Parse(gId);
                        var group = RepositoryProvider.Get<GroupRepository>()
                            .FirstOrDefault(p => p.GroupId == id);
                        if (group != null)
                        {
                            user.JoinedGroups.Add(group);
                        }
                    }
                }
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    AddErrors(result);
                    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                        ? "/User"
                        : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                }
                else
                {
                    return
                        Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/User"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                }
            }
            ViewBag.ReturnUrl = Request.QueryString["returnUrl"];
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View(model);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = UserManager.FindById(id);
            var isSuccess = RepositoryProvider.Get<SPRepository>().DeleteUserById(id);
            if (isSuccess)
            {
                var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                if (!string.IsNullOrEmpty(user.PhotoPath))
                {
                    await azureStorageHelper.DeleteFile(user.PhotoPath, AzureStorageHelper.FileUsage.UserPhotos);
                }
            }
            return Redirect(Request.UrlReferrer == null ? "/User" : Request.UrlReferrer.AbsoluteUri);
        }
    }
}
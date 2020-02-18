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
using GoodApp.Data.Models;
using GoodApp.Data.Repositories;
using GoodApp.Data.Views;
using Microsoft.AspNet.Identity;

namespace GoodApp.Backend.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CodeController : BaseMvcController
    {
        private readonly string[] _columns = new[]
        {"Name", "Code", "CreateDate", "CreatorFirstName", "MembersCount", "ChallengesCount"};

        public ActionResult Index(string type = "ALL", string id = "ALL", int pageIndex = 0, int pageSize = 0,
            string search = "", string orderBy = "CreateDate", bool isAsc = false)
        {
            int count = 0;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 20 : pageSize;
            orderBy = (string.IsNullOrEmpty(orderBy) ||
                       _columns.All(p => !String.Equals(p, orderBy, StringComparison.CurrentCultureIgnoreCase)))
                ? "CreateDate"
                : orderBy;
            string subTitle = string.Empty;
            string preLink = "/Code/?type=" + type.ToUpper() + "&id=" + id;
            IEnumerable<GroupView> codeViews;
            if (type.ToUpper() == "CHALLENGE" && !string.IsNullOrEmpty(id))
            {
                var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Challenge'>Challenges</a>&gt;<a href='/Challenge?type=SINGLE&id=" +
                           challenge.ChallengeId + "'>" + challenge.Name + "</a>&gt;Group Codes";
            }
            else if (type.ToUpper() == "USER" && !string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);
                subTitle = "<a href='/User'>Users</a>&gt;<a href='/User?type=SINGLE&id=" + user.Id + "'>" +
                           user.FirstName + " " + user.LastName + "</a>&gt;Group Codes";
            }
            else if (type.ToUpper() == "SINGLE" && !string.IsNullOrEmpty(id))
            {
                var code = RepositoryProvider.Get<GroupRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Code'>Group Codes</a>&gt;" + code.Code;
            }
            codeViews = RepositoryProvider.Get<ViewRepository>()
                .GetCodeViews(ref count, pageIndex, pageSize, search, orderBy, type.ToUpper(), id, isAsc);

            ViewBag.PreLink = preLink;
            ViewBag.SubTitle = subTitle;
            ViewBag.Type = type.ToUpper();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.ItemCount = count;
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.IsAsc = isAsc;
            return View(codeViews);
        }

        public ActionResult Create()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/Code" : Request.UrlReferrer.AbsoluteUri;
            EditGroupBindingModel model = new EditGroupBindingModel();
            model.Code = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Name,Description,Code,Picture")] EditGroupBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var group =
                    RepositoryProvider.Get<GroupRepository>()
                        .FirstOrDefault(
                            p => p.Name.ToUpper() == model.Name.ToUpper() || p.Code.ToUpper() == model.Code.ToUpper());
                if (group != null)
                {
                    ModelState.AddModelError("", "Name or Code has been used.");
                    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                        ? "/Code"
                        : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                }
                else
                {
                    group = new Group()
                    {
                        GroupId = Guid.NewGuid(),
                        Name = model.Name,
                        Description = model.Description,
                        Code = model.Code,
                        CreateDate = DateTime.UtcNow,
                        CreatorId = CurrentAccess.UserId
                    };

                    if (model.Picture != null && model.Picture.ContentLength > 0)
                    {
                        var stream = new MemoryStream();
                        ImageHelper.Resize(model.Picture.InputStream, stream, 300, ImageFormat.Jpeg);
                        var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                        var newPicture = await azureStorageHelper.SaveFileStream(stream, Guid.NewGuid() + ".jpg",
                            AzureStorageHelper.FileUsage.UserPhotos);
                        group.Picture = newPicture;
                    }

                    RepositoryProvider.Get<GroupRepository>().Insert(group);
                    RepositoryProvider.Save();
                    return
                        Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/Code"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                }
            }
            return View(model);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/Code" : Request.UrlReferrer.AbsoluteUri;
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var group = RepositoryProvider.Get<GroupRepository>().GetById(Guid.Parse(id));
            if (group == null)
            {
                return HttpNotFound();
            }
            var editGroup = new EditGroupBindingModel()
            {
                GroupId = group.GroupId,
                Name = group.Name,
                Description = group.Description,
                Code = group.Code,
                PictureUrl = group.Picture
            };
            return View(editGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "GroupId,Name,Description,Code,Picture,PictureUrl")] EditGroupBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var group =
                    RepositoryProvider.Get<GroupRepository>()
                        .FirstOrDefault(
                            p => p.Name.ToUpper() == model.Name.ToUpper() || p.Code.ToUpper() == model.Code.ToUpper());
                if (group != null && group.GroupId != model.GroupId)
                {
                    ModelState.AddModelError("", "Name or Code has been used.");
                    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                        ? "/Code"
                        : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                }
                else
                {
                    group = RepositoryProvider.Get<GroupRepository>().GetById(model.GroupId.GetValueOrDefault());
                    group.Name = model.Name;
                    group.Description = model.Description;
                    group.Code = model.Code;

                    var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                    if (model.Picture != null && model.Picture.ContentLength > 0)
                    {
                        var stream = new MemoryStream();
                        ImageHelper.Resize(model.Picture.InputStream, stream, 300, ImageFormat.Jpeg);

                        var newPicture = await azureStorageHelper.SaveFileStream(stream, Guid.NewGuid() + ".jpg",
                            AzureStorageHelper.FileUsage.UserPhotos);
                        group.Picture = newPicture;
                    }

                    RepositoryProvider.Get<GroupRepository>().Update(group);
                    RepositoryProvider.Save();
                    if (!string.IsNullOrEmpty(model.PictureUrl))
                    {
                        await azureStorageHelper.DeleteFile(model.PictureUrl, AzureStorageHelper.FileUsage.UserPhotos);
                    }
                    return
                        Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/Code"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                }
            }
            return View(model);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            RepositoryProvider.Get<SPRepository>().DeleteGroupById(id);
            return Redirect(Request.UrlReferrer == null ? "/Code" : Request.UrlReferrer.AbsoluteUri);
        }
    }
}
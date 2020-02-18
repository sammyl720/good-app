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

namespace GoodApp.Backend.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ChallengeController : BaseMvcController
    {
        private readonly string[] _columns = new[]
        {
            "Name", "Type", "Count", "DueDate", "Order", "Status", "FrequencyCount", "FrequencyValue", "FrequencyType",
            "CreateDate", "CreatorFirstName", "ChallengeGroupCount", "DeedCount", "ValidDeedCount", "ChallengeUserCount"
        };

        public ActionResult Index(string type = "ALL", string id = "ALL", int pageIndex = 0, int pageSize = 0,
            string status = "All", string search = "", string orderBy = "Order", bool isAsc = false, string code = "All")
        {
            int count = 0;
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 20 : pageSize;
            Enums.ChallengeStatus challengeStatus;
            status = string.IsNullOrEmpty(status) || !Enum.TryParse(status, out challengeStatus)
                ? "All"
                : challengeStatus.ToString();
            code = !string.IsNullOrEmpty(code) && code.ToUpper() != "ALL" ? code : "All";
            orderBy = (string.IsNullOrEmpty(orderBy) ||
                       _columns.All(p => !String.Equals(p, orderBy, StringComparison.CurrentCultureIgnoreCase)))
                ? "Order"
                : orderBy;
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();

            string subTitle = string.Empty;
            string preLink = "/Challenge/?type=" + type.ToUpper() + "&id=" + id;
            IEnumerable<ChallengeView> challengeViews;
            string groupCode = string.Empty;
            if (type.ToUpper() == "CODE" && !string.IsNullOrEmpty(id))
            {
                var group = RepositoryProvider.Get<GroupRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Code'>Group Codes</a>&gt;<a href='/Code?type=SINGLE&id=" + group.GroupId + "'>" +
                           group.Code + "</a>&gt;Challenges";
                groupCode = group.Code;
            }
            else if (type.ToUpper() == "USER" && !string.IsNullOrEmpty(id))
            {
                var user = UserManager.FindById(id);
                subTitle = "<a href='/User'>Users</a>&gt;<a href='/User?type=SINGLE&id=" + user.Id + "'>" +
                           user.FirstName + " " + user.LastName + "</a>&gt;Challenges";
            }
            else if (type.ToUpper() == "SINGLE" && !string.IsNullOrEmpty(id))
            {
                var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(id));
                subTitle = "<a href='/Challenge'>Challenges</a>&gt;" + challenge.Name;
            }
            challengeViews = RepositoryProvider.Get<ViewRepository>()
                .GetChallengeViews(ref count, pageIndex, pageSize, status, search, orderBy,
                    string.IsNullOrEmpty(groupCode) ? code : groupCode, type.ToUpper(), id, isAsc);
            ViewBag.PreLink = preLink;
            ViewBag.SubTitle = subTitle;
            ViewBag.Type = type.ToUpper();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.ItemCount = count;
            ViewBag.Status = status;
            ViewBag.DisplayStatus = status.ToUpper() == "ALL" ? "All Statuses" : status;
            ViewBag.Code = code;
            ViewBag.DisplayCode = code.ToUpper() == "ALL" ? "All Group Codes" : code;
            ViewBag.Search = search;
            ViewBag.OrderBy = orderBy;
            ViewBag.IsAsc = isAsc;
            return View(challengeViews);
        }

        public ActionResult Create()
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/Challenge" : Request.UrlReferrer.AbsoluteUri;
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "Name,Type,Count,Description,DueDate,Status,GroupIds,Picture")] EditChallengeBindingModel
                model)
        {
            //FrequencyValue,FrequencyCount,FrequencyType,
            if (ModelState.IsValid)
            {
                //var challenge =
                //    RepositoryProvider.Get<ChallengeRepository>()
                //        .FirstOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper());
                //if (challenge != null)
                //{
                //    ModelState.AddModelError("", "Challenge has been used.");
                //    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                //            ? "/Challenge"
                //            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                //}
                //else
                {
                    var challenge = new Challenge()
                    {
                        ChallengeId = Guid.NewGuid(),
                        Name = model.Name,
                        Count = model.Count,
                        Description = model.Description,
                        DueDate = model.DueDate.AddDays(1).AddSeconds(-1).ToUniversalTime(),
                        FrequencyValue = 0, //model.FrequencyValue,
                        FrequencyCount = 0, //model.FrequencyCount,
                        CreateDate = DateTime.UtcNow,
                        CreatorId = CurrentAccess.UserId,
                        Type = model.Type.ToString(),
                        Status = model.Status.ToString(),
                        FrequencyType = Enums.FrequencyType.ByDay.ToString(), // model.FrequencyType.ToString(),
                        Order = RepositoryProvider.Get<ChallengeRepository>().GetNextOrder()
                    };
                    if (model.GroupIds != null && model.GroupIds.Length > 0)
                    {
                        foreach (var gId in model.GroupIds)
                        {
                            Guid id = Guid.Parse(gId);
                            var group = RepositoryProvider.Get<GroupRepository>()
                                .FirstOrDefault(p => p.GroupId == id);
                            if (group != null)
                            {
                                challenge.Groups.Add(group);
                            }
                        }
                    }
                    if (model.Picture != null && model.Picture.ContentLength > 0)
                    {
                        var stream = new MemoryStream();
                        ImageHelper.Resize(model.Picture.InputStream, stream, 300, ImageFormat.Jpeg);
                        var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                        var newPicture = await azureStorageHelper.SaveFileStream(stream, Guid.NewGuid() + ".jpg",
                            AzureStorageHelper.FileUsage.UserPhotos);
                        challenge.Picture = newPicture;
                    }

                    RepositoryProvider.Get<ChallengeRepository>().Insert(challenge);
                    RepositoryProvider.Save();
                    return
                        Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/Challenge"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                }
            }
            ViewBag.ReturnUrl = Request.QueryString["returnUrl"];
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View(model);
        }

        [HttpPost]
        public void ReOrder(Guid challengeId, int diff, bool isAsc)
        {
            RepositoryProvider.Get<SPRepository>().ReOrderChallenge(challengeId, diff, isAsc);
        }

        public ActionResult Edit(string id)
        {
            ViewBag.ReturnUrl = Request.UrlReferrer == null ? "/Challenge" : Request.UrlReferrer.AbsoluteUri;
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(Guid.Parse(id));
            if (challenge == null)
            {
                return HttpNotFound();
            }
            var editChallenge = new EditChallengeBindingModel()
            {
                ChallengeId = challenge.ChallengeId,
                Name = challenge.Name,
                Count = challenge.Count,
                Description = challenge.Description,
                DueDate = challenge.DueDate.ToLocalTime(),
                //FrequencyValue = challenge.FrequencyValue,
                //FrequencyCount = challenge.FrequencyCount,
                PictureUrl = challenge.Picture,
                GroupIds = challenge.Groups.Any() ? challenge.Groups.Select(p => p.GroupId.ToString()).ToArray() : null
            };
            Enums.ChallengeStatus challengeStatus;
            editChallenge.Status = Enum.TryParse(challenge.Status, out challengeStatus)
                ? challengeStatus
                : Enums.ChallengeStatus.Draft;

            Enums.ChallengeType challengeType;
            editChallenge.Type = Enum.TryParse(challenge.Type, out challengeType)
                ? challengeType
                : Enums.ChallengeType.Group;

            //Enums.FrequencyType frequencyType;
            //editChallenge.FrequencyType = Enum.TryParse(challenge.FrequencyType, out frequencyType)
            //    ? frequencyType
            //    : Enums.FrequencyType.ByDay;

            return View(editChallenge);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "ChallengeId,Name,Type,Count,Description,DueDate,Status,GroupIds,PictureUrl,Picture")] EditChallengeBindingModel model)
        {
            //FrequencyValue,FrequencyCount,FrequencyType,
            if (ModelState.IsValid)
            {
                //var challenge =
                //    RepositoryProvider.Get<ChallengeRepository>()
                //        .FirstOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper());
                //if (challenge != null && challenge.ChallengeId != model.ChallengeId)
                //{
                //    ModelState.AddModelError("", "Challenge Name has been used.");
                //    ViewBag.ReturnUrl = string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                //            ? "/Challenge"
                //            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]);
                //}
                //else
                {
                    var challenge = RepositoryProvider.Get<ChallengeRepository>().GetById(model.ChallengeId);
                    challenge.Name = model.Name;
                    challenge.Count = model.Count;
                    challenge.Description = model.Description;
                    challenge.DueDate = model.DueDate.AddDays(1).AddSeconds(-1).ToUniversalTime();
                    //challenge.FrequencyValue = model.FrequencyValue;
                    //challenge.FrequencyCount = model.FrequencyCount;
                    challenge.Type = model.Type.ToString();
                    challenge.Status = model.Status.ToString();
                    //challenge.FrequencyType = model.FrequencyType.ToString();

                    if (model.GroupIds != null && model.GroupIds.Length > 0)
                    {
                        challenge.Groups.Clear();
                        foreach (var gId in model.GroupIds)
                        {
                            Guid id = Guid.Parse(gId);
                            var group = RepositoryProvider.Get<GroupRepository>()
                                .FirstOrDefault(p => p.GroupId == id);
                            if (group != null)
                            {
                                challenge.Groups.Add(group);
                            }
                        }
                    }
                    var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                    if (model.Picture != null && model.Picture.ContentLength > 0)
                    {
                        var stream = new MemoryStream();
                        ImageHelper.Resize(model.Picture.InputStream, stream, 300, ImageFormat.Jpeg);

                        var newPicture = await azureStorageHelper.SaveFileStream(stream, Guid.NewGuid() + ".jpg",
                            AzureStorageHelper.FileUsage.UserPhotos);
                        challenge.Picture = newPicture;
                    }

                    RepositoryProvider.Get<ChallengeRepository>().Update(challenge);
                    RepositoryProvider.Save();
                    if (!string.IsNullOrEmpty(model.PictureUrl))
                    {
                        await azureStorageHelper.DeleteFile(model.PictureUrl, AzureStorageHelper.FileUsage.UserPhotos);
                    }
                    return
                        Redirect(string.IsNullOrEmpty(Request.QueryString["returnUrl"])
                            ? "/Challenge"
                            : HttpUtility.UrlDecode(Request.QueryString["returnUrl"]));
                }
            }
            ViewBag.ReturnUrl = Request.QueryString["returnUrl"];
            ViewBag.Groups = RepositoryProvider.Get<GroupRepository>().Get().ToList();
            return View(model);
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            RepositoryProvider.Get<SPRepository>().DeleteChallengeById(id);
            return Redirect(Request.UrlReferrer == null ? "/Challenge" : Request.UrlReferrer.AbsoluteUri);
        }
    }
}
using System;
using GoodApp.Backend.Api.Controllers;
using GoodApp.Backend.Models;
using GoodApp.Data.Models;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GoodApp.Backend.Helpers;
using GoodApp.Data.Repositories;

namespace GoodApp.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UserApiController : BaseApiController
    {
        public UserApiController()
        {
        }

        public UserApiController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        [Route("me")]
        public async Task<IHttpActionResult> GetUserInfo()
        {
            var user = await UserManager.FindByIdAsync(CurrentAccess.UserId);
            return Ok(UserDetailViewModel.Create(user));
        }

        [Route("ingroup")]
        public async Task<IHttpActionResult> GetUsersInGroup(string q = "", int pageIndex = 0, int pageSize = 0)
        {
            var user = await RepositoryProvider.UserStore.FindByIdAsync(CurrentAccess.UserId);
            if (user == null)
            {
                return NotFound();
            }
            var groupIds = user.JoinedGroups.Select(p => p.GroupId).ToList();

            var recentTaggedUsers = RepositoryProvider.Get<DeedRepository>()
                .Get(p => p.CreatorId == CurrentAccess.UserId && p.TaggedUsers.Any())
                .OrderByDescending(p => p.CreateDate)
                .SelectMany(p => p.TaggedUsers)
                .Distinct();

            var groupUser = RepositoryProvider.Get<GroupRepository>()
                .Get(p => groupIds.Contains(p.GroupId))
                .SelectMany(p => p.Members)
                .OrderBy(p => p.FirstName);

            var result = recentTaggedUsers.Concat(groupUser)
                .Distinct()
                .Where(p => p.Id != CurrentAccess.UserId)
                .Include(p => p.JoinedGroups);

            if (!string.IsNullOrEmpty(q))
            {
                result = SearchUser(result.AsQueryable(), q);
            }

            if (pageSize > 0)
            {
                result = result.AsEnumerable().Skip(pageIndex*pageSize).Take(pageSize).AsQueryable();
            }

            return Ok(result.Select(UserDetailViewModel.Create));
        }

        [HttpPost]
        [Route("changePhoto")]
        public async Task<IHttpActionResult> ChangePhoto(ChangePhotoBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await RepositoryProvider.UserStore.FindByIdAsync(CurrentAccess.UserId);
                var oldPhotoPath = user.PhotoPath;

                Stream s = new MemoryStream(model.Photo.Buffer);
                ImageHelper.Resize(s, s, 300, ImageFormat.Jpeg);
                var azureStorageHelper = new AzureStorageHelper(ConfigHelper.AzureStorageConnectionString);
                var newPhotoPath = await azureStorageHelper.SaveFileStream(s, Guid.NewGuid() + ".jpg",
                    AzureStorageHelper.FileUsage.UserPhotos);

                user.PhotoPath = newPhotoPath;
                await UserManager.UpdateAsync(user);

                if (!string.IsNullOrEmpty(oldPhotoPath))
                {
                    await azureStorageHelper.DeleteFile(oldPhotoPath, AzureStorageHelper.FileUsage.UserPhotos);
                }

                return Ok(new {photoPath = newPhotoPath});
            }

            return BadRequest();
        }

        private IQueryable<ApplicationUser> SearchUser(IQueryable<ApplicationUser> users, string q)
        {
            var keys = q.Split(new char[] {' '});
            return keys.Aggregate(users,
                (current, key) =>
                    current.Where(
                        p =>
                            p.FirstName.ToUpper().Contains(key.ToUpper()) ||
                            p.LastName.ToUpper().Contains(key.ToUpper())));
        }
    }
}
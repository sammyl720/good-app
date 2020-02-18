using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GoodApp.Backend.Api.Controllers;
using GoodApp.Backend.Models;
using GoodApp.Data.Repositories;

namespace GoodApp.Backend.Controllers
{
    [RoutePrefix("api/deeds")]
    [Authorize]
    public class DeedApiController : BaseApiController
    {
        [Route("overview")]
        [HttpGet]
        public async Task<IHttpActionResult> Overview()
        {
            var myTotalDeedCount = await
                RepositoryProvider.Get<DeedRepository>().Get(p => p.CreatorId == CurrentAccess.UserId).CountAsync();

            var networkDeedCount = await
                RepositoryProvider.Get<DeedRepository>().Get(
                    p => p.Challenge.Groups.Any(x => x.Members.Any(y => y.Id == CurrentAccess.UserId))).CountAsync();

            var totalDeedCount = await RepositoryProvider.Get<DeedRepository>().Get().CountAsync();

            var friendsCount = (await RepositoryProvider.UserStore.FindByIdAsync(CurrentAccess.UserId))
                .JoinedGroups.SelectMany(p => p.Members).Count(p => p.Id != CurrentAccess.UserId);


            var result = new DeedOverViewModel
            {
                MyTotalDeedCount = myTotalDeedCount,
                NetworkDeedCount = networkDeedCount,
                TotalDeedCount = totalDeedCount,
                FriendsCount = friendsCount
            };

            return Ok(result);
        }
    }
}

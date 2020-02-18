using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using GoodApp.Backend.Api.Controllers;
using GoodApp.Backend.Models;
using GoodApp.Data.Repositories;

namespace GoodApp.Backend.Controllers
{
    [Authorize]
    [RoutePrefix("api/groups")]
    public class GroupApiController : BaseApiController
    {
        [Route("{code}/join")]
        [HttpPost]
        public async Task<IHttpActionResult> Join(string code)
        {
            try
            {
                await RepositoryProvider.Get<GroupRepository>().JoinGroup(CurrentAccess.UserId, code);
                await RepositoryProvider.SaveAsync();
            }
            catch (GroupRepository.GroupNotFoundException)
            {
                return BadRequest("Group not found.");
            }
            catch (GroupRepository.AlreadyInGroupException)
            {
                return BadRequest("Already joined this group.");
            }

            return Ok();
        }

        [Route("mine")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyGroup()
        {
            var group =
                await
                    RepositoryProvider.Get<GroupRepository>().FirstOrDefaultAsync(
                        p => p.Members.Any(g => g.Id == CurrentAccess.UserId));
            if (group == null)
            {
                return NotFound();
            }

            return Ok(GroupViewModel.Create(group));
        }
    }
}

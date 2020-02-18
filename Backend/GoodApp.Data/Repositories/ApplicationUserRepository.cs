using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GoodApp.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Data.Repositories
{
    public class ApplicationUserRepository : TableRepository<ApplicationUser>
    {
        public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<ApplicationUser> GetUsersHaveChallenge(Guid challengeId)
        {
            return DbSet.Where(user =>
                    user.JoinedGroups.Any(
                        group => group.Challenges.Any(
                            challenge => challenge.ChallengeId == challengeId)));
        }
    }
}

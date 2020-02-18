using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data.Views;

namespace GoodApp.Data.Repositories
{
    public class SPRepository : BaseRepository
    {
        public SPRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public bool DeleteUserById(string userId)
        {
            return this.ExecuteRawSql("SP_DeleteUserById @UserId", new SqlParameter("UserId", userId)) > 0;
        }

        public bool DeleteGroupById(string groupId)
        {
            return this.ExecuteRawSql("SP_DeleteGroupById @GroupId", new SqlParameter("GroupId", groupId)) > 0;
        }

        public bool DeleteChallengeById(string challengeId)
        {
            return this.ExecuteRawSql("SP_DeleteChallengeById @ChallengeId", new SqlParameter("ChallengeId", challengeId)) > 0;
        }

        public void ReOrderChallenge(Guid challengeId, int diff, bool isAsc)
        {
            this.ExecuteRawSql("SP_ReOrderChallenge @ChallengeId, @Diff, @IsAsc",
                new SqlParameter("ChallengeId", challengeId), new SqlParameter("Diff",diff),
                new SqlParameter("IsAsc",isAsc));
        }
    }
}

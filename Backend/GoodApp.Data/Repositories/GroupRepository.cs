using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using GoodApp.Data.Models;

namespace GoodApp.Data.Repositories
{
    public class GroupRepository : TableRepository<Group>
    {
        public GroupRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task JoinGroup(string userId, string groupCode)
        {
            var group = await FirstOrDefaultAsync(p => p.Code.ToUpper() == groupCode.ToUpper());
            if (group == null)
            {
                throw new GroupNotFoundException();
            }

            if (group.Members.Any(p => p.Id == userId))
            {
                throw new AlreadyInGroupException();
            }

            DbContext.Database.ExecuteSqlCommand("insert into GroupMembers (GroupId,UserId) values (@p0,@p1)",
                new SqlParameter("@p0", group.GroupId), new SqlParameter("@p1", userId));

            Update(group);
        }

        public bool ExistCode(string groupCode)
        {
            var group = FirstOrDefault(p => p.Code.ToUpper() == groupCode.ToUpper());
            if (group == null)
            {
                return false;
            }
            return true;
        }

        public class GroupNotFoundException : Exception
        {

        }

        public class AlreadyInGroupException : Exception
        {

        }
    }
}

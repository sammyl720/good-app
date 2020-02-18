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
    public class ViewRepository : BaseRepository
    {
        public ViewRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public IEnumerable<DeedView> GetDeedViews(ref int count, int pageIndex, int pageSize, string search, string orderBy, string challengeId, string userId, string type, string subType, string id,bool isAsc = true)
        {
            IEnumerable<DeedView> deeds;
            var sqlString = "Select * From AllDeedViews Where 1=1 ";
            switch (type)
            {
                case "CHALLENGE":
                    sqlString += (subType.ToUpper() == "VALID" ? " And IsValid =1 " : (subType.ToUpper() == "NOTVALID" ? " And IsValid=0 " : " "));
                    sqlString +=" And ChallengeId = '" + id + "' " + (userId != "All" ? " And CreatorId = '" + userId + "' " : "");
                    break;
                case "USER":
                    sqlString += (subType.ToUpper() == "VALID" ? " And IsValid =1 " : (subType.ToUpper() == "NOTVALID" ? " And IsValid=0 " : " "));
                    sqlString += (subType.ToUpper() == "NETWORK"
                        ? " And CreatorId In (Select UserId From GroupMembers Where GroupId In (Select GroupId From GroupMembers Where UserId ='" + id + "')) " : " And CreatorId = '" + id + "' ");
                    sqlString += (challengeId != "All" ? " And ChallengeId ='" + challengeId + "' " : "");
                    break;
                case "SINGLE":
                    sqlString += (subType.ToUpper() == "VALID" ? " And IsValid =1 " : (subType.ToUpper() == "NOTVALID" ? " And IsValid=0 " : " "));
                    sqlString += " And DeedId = '" + id + "' ";
                    sqlString += (challengeId != "All"? " And ChallengeId ='" + challengeId + "' ": "");      
                    break;
                default:
                    sqlString += (challengeId != "All"? " And ChallengeId ='" + challengeId + "' " : "");
                    sqlString += (userId != "All" ? " And CreatorId = '" + userId + "' " : "");
                    break;
            }
            deeds = this.ExecuteSqlQueryList<DeedView>(sqlString + " Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
            if (!string.IsNullOrEmpty(search))
            {
                deeds =
                    deeds.Where(p => p.ChallengeName.Contains(search) || p.Location.Contains(search));
            }
            count = deeds.Count();
            return deeds.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
        
        public IEnumerable<GroupView> GetCodeViews(ref int count, int pageIndex, int pageSize, string search, string orderBy, string type,string id, bool isAsc = true)
        { 
            IEnumerable<GroupView> groups;
            switch (type)
            {
                case "CHALLENGE":
                    groups = this.ExecuteSqlQueryList<GroupView>("Select * From AllGroupViews Where GroupId In (Select Distinct GroupId From ChallengeGroups Where ChallengeId ='" + id + "') Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "USER":
                    groups = this.ExecuteSqlQueryList<GroupView>("Select * From AllGroupViews Where GroupId In (Select Distinct GroupId From GroupMembers Where UserId='" + id + "') Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "SINGLE":
                    groups = this.ExecuteSqlQueryList<GroupView>("Select * From AllGroupViews Where GroupId='" + id + "' Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                default:
                    groups = this.ExecuteSqlQueryList<GroupView>("Select * From AllGroupViews Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
            }
            if (!string.IsNullOrEmpty(search))
            {
                groups =
                    groups.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }
            count = groups.Count();
            return groups.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<UserView> GetUserViews(ref int count, int pageIndex, int pageSize, string status, string search, string orderBy, string type, string id, bool isAsc = true)
        {
            IEnumerable<UserView> users;
            switch (type)
            {
                case "DEED":
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Where Id In (Select UserId From TagUsers Where DeedId = '" + id + "')  Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "CHALLENGE":
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Where Id In (Select UserId From GroupMembers Where GroupId In (Select GroupId From ChallengeGroups Where ChallengeId = '" + id + "')) Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "USER":
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Where Id In (Select Distinct UserId From TagUsers Where DeedId In (Select DeedId From Deeds Where CreatorId = '" + id + "')) Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "CODE":
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Where Id In (Select UserId From GroupMembers Where GroupId = '" + id + "') Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                case "SINGLE":
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Where Id = '" + id + "' Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
                default:
                    users = this.ExecuteSqlQueryList<UserView>("Select * From AllUserViews Order By " + orderBy + (isAsc ? " Asc" : " Desc"));
                    break;
            }
            if (status != "All")
            {
                users = users.Where(p => p.Status == status);
            }
            if (!string.IsNullOrEmpty(search))
            {
                users =
                    users.Where(
                        p => p.UserName.Contains(search) || p.FirstName.Contains(search) || p.LastName.Contains(search));
            }
            count = users.Count();
            return users.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public IEnumerable<ChallengeView> GetChallengeViews(ref int count, int pageIndex, int pageSize, string status, string search, string orderBy, string code, string type, string id, bool isAsc = true)
        {
            IEnumerable<ChallengeView> challenges;
            if (code != "All")
            {
                switch (type)
                {
                    case "USER":
                        challenges = this.ExecuteSqlQueryList<ChallengeView>("Select c.* from AllChallengeViews c Inner Join ChallengeGroups cg On c.ChallengeId = cg.ChallengeId "
                    + "Inner Join AllGroupViews g On g.GroupId = cg.GroupId And g.GroupId In (Select GroupId From GroupMembers Where UserId ='" + id + "') "
                    + "Where Upper(g.Code) = '" + code.ToUpper() + "' Order By c.[" + orderBy + "] " + (isAsc ? " Asc" : " Desc"));
                        break;
                    case "SINGLE":
                        challenges = this.ExecuteSqlQueryList<ChallengeView>("Select c.* from AllChallengeViews c Inner Join ChallengeGroups cg On c.ChallengeId = cg.ChallengeId "
                   + "Inner Join AllGroupViews g On g.GroupId = cg.GroupId "
                   + "Where c.ChallengeId ='" + id + "' And Upper(g.Code) = '" + code.ToUpper() + "' Order By c.[" + orderBy + "] " + (isAsc ? " Asc" : " Desc"));
                        break;
                    default:
                        challenges = this.ExecuteSqlQueryList<ChallengeView>("Select c.* from AllChallengeViews c Inner Join ChallengeGroups cg On c.ChallengeId = cg.ChallengeId "
                    + "Inner Join AllGroupViews g On g.GroupId = cg.GroupId "
                    + "Where Upper(g.Code) = '" + code.ToUpper() + "' Order By c.[" + orderBy + "] " + (isAsc ? " Asc" : " Desc"));
                        break;
                }

            }
            else
            {
                switch (type)
                {
                    case "USER":
                        challenges =
                   this.ExecuteSqlQueryList<ChallengeView>("Select * From AllChallengeViews Where ChallengeId In (Select ChallengeId From ChallengeGroups Where GroupId In (Select GroupId From GroupMembers Where UserId = '" + id + "')) Order By [" + orderBy +"] " + (isAsc ? " Asc" : " Desc"));
                        break;
                    case "SINGLE":
                        challenges =
                    this.ExecuteSqlQueryList<ChallengeView>("Select * From AllChallengeViews Where ChallengeId = '" + id + "' Order By [" + orderBy +"] " + (isAsc ? " Asc" : " Desc"));
                        break;
                    default:
                        challenges = this.ExecuteSqlQueryList<ChallengeView>("Select * From AllChallengeViews Order By [" + orderBy + "] " + (isAsc ? " Asc" : " Desc"));
                        break;
                }
            }
            if (status != "All")
            {
                challenges = challenges.Where(p => p.Status == status);
            }
            if (!string.IsNullOrEmpty(search))
            {
                challenges =
                    challenges.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }
            count = challenges.Count();
            return challenges.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}

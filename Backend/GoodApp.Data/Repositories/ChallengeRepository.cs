using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GoodApp.Data.Models;

namespace GoodApp.Data.Repositories
{
    public class ChallengeRepository : TableRepository<Challenge>
    {
        public ChallengeRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IEnumerable<Challenge> GetAvailableChallenges(string userId)
        {
            return
                Get(p => p.Groups.Any(x => x.Members.Any(y => y.Id == userId)) && p.DueDate > DateTime.UtcNow)
                    .Include("Deeds")
                    .OrderBy(p => p.CreateDate);
        }

        public int GetNextOrder()
        {
            return DbSet.Max(p => p.Order) + 1;
        }
    }
}

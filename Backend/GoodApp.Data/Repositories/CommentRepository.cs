using System.Data.Entity;
using GoodApp.Data.Models;
using System.Linq;

namespace GoodApp.Data.Repositories
{
    public class CommentRepository : TableRepository<Comment>
    {
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Comment> GetComments(string referenceId)
        {
            return Get(p => p.ReferenceId == referenceId).Include(p => p.User).OrderBy(p => p.CreateDate);
        }
    }
}
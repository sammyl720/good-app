using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GoodApp.Data
{
    public abstract class BaseRepository : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private bool _disposed;

        protected BaseRepository(ApplicationDbContext dbContext)
        {
            _disposed = dbContext != null;
            _dbContext = dbContext ?? new ApplicationDbContext();
        }

        protected ApplicationDbContext DbContext
        {
            get { return _dbContext; }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual int ExecuteRawSql(string sql, params object[] parameters)
        {
            return DbContext.Database.ExecuteSqlCommand(sql, parameters);
        }

        public virtual T ExecuteSqlQuery<T>(string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<T>(sql, parameters).FirstOrDefault();
        }

        public virtual IEnumerable<T> ExecuteSqlQueryList<T>(string sql, params object[] parameters)
        {
            return DbContext.Database.SqlQuery<T>(sql, parameters);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }
    }

    public class TableRepository<TEntity> : BaseRepository where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        protected DbSet<TEntity> DbSet {get { return _dbSet; }}

        public TableRepository(ApplicationDbContext context) : base(context)
        {
            _dbSet = DbContext.Set<TEntity>();
        }

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return orderBy != null ? orderBy(query) : query;
        }

        public virtual TEntity GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }
    }
}
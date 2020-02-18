using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Threading.Tasks;
using GoodApp.Data.Models;
using GoodApp.Data.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GoodApp.Data
{
    public class RepositoryProvider : IDisposable
    {
        private bool _disposed;
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<string, BaseRepository> _types = new Dictionary<string, BaseRepository>();
        private readonly UserStore<ApplicationUser> _userStore;
        
        private readonly RoleStore<IdentityRole> _roleStore; 
        private RepositoryProvider()
        {
            _dbContext = new ApplicationDbContext();
            _userStore = new UserStore<ApplicationUser>(_dbContext);
            _roleStore = new RoleStore<IdentityRole>(_dbContext);
        }
        public static RepositoryProvider Create()
        {
            return new RepositoryProvider();
        }
        public UserStore<ApplicationUser> UserStore {
            get { return _userStore; }
        }

        public RoleStore<IdentityRole> RoleStore
        {
            get { return _roleStore; }
        }
        public void Save()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult entityValidationError in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",entityValidationError.Entry.Entity.GetType().Name,entityValidationError.Entry.State);
                    foreach (DbValidationError dbValidationError in entityValidationError.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",dbValidationError.PropertyName,dbValidationError.ErrorMessage);
                    }
                }
                throw;
            }
        }
        public Task<int> SaveAsync()
        {
            try
            {
                return _dbContext.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult entityValidationError in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",entityValidationError.Entry.Entity.GetType().Name,entityValidationError.Entry.State);
                    foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",validationError.PropertyName,validationError.ErrorMessage);
                    }
                }
                throw;
            }
        }
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                    foreach (var baseRepository in _types.Values)
                    {
                        baseRepository.Dispose();
                    }
                }
            }
            _disposed = true;
        }
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public T Get<T>() where T : BaseRepository
        {
            string name = typeof(T).FullName;
            if (_types.ContainsKey(name))
            {
                return _types[name] as T;
            }
            var t = Activator.CreateInstance(typeof(T), _dbContext) as T;
            _types.Add(name, t);
            return t;
        }
    }
}

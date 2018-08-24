using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbLogger.Core.Context
{
    public interface ILoggerUnitOfWork: IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsModified<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class;
        void ExecuteSqlCommand(string query);
        void ExecuteSqlCommand(string query, params object[] parameters);

        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Migrate();

    }
}

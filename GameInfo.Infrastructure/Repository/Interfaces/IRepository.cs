using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Infrastructure.Repository.Interfaces
{
    public interface IRepository : IDisposable
    {
        Task<TEntity> GetById<TEntity>(int Id) where TEntity : class;

        Task<TEntity> GetById<TEntity>(long Id) where TEntity : class;

        IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        TEntity Add<TEntity>(TEntity entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        Task BulkInsert<TEntity>(List<TEntity> entity) where TEntity : class;
        Task BulkUpdate<TEntity>(List<TEntity> entity) where TEntity : class;


        Task<int> Commit();
    }
}

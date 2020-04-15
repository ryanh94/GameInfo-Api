using EFCore.BulkExtensions;
using GameInfo.Infrastructure.Repository.Context;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Infrastructure.Repository
{
    public class GameInfoRepository : IRepository
    {
        private readonly GameInfoContext DbContext;

        public GameInfoRepository(GameInfoContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<TEntity> GetById<TEntity>(int Id) where TEntity : class
        {
            TEntity query = await DbContext.Set<TEntity>().FindAsync(Id);

            return query;
        }

        public async Task<TEntity> GetById<TEntity>(long Id) where TEntity : class
        {
            TEntity query = await DbContext.Set<TEntity>().FindAsync(Id);

            return query;
        }

        public IQueryable<TEntity> Get<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>().Where(predicate).AsNoTracking();
            return query;
        }

        public TEntity Add<TEntity>(TEntity entity) where TEntity : class
        {
            return DbContext.Set<TEntity>().Add(entity).Entity;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            DbContext.Set<TEntity>().Remove(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            DbContext.Set<TEntity>().Update(entity);
        }
        public async Task BulkInsert<TEntity>(List<TEntity> entity) where TEntity : class
        {
            await DbContext.BulkInsertAsync(entity);
        }
        public async Task BulkUpdate<TEntity>(List<TEntity> entity) where TEntity : class
        {
            await DbContext.BulkUpdateAsync(entity);
        }
        public async Task<int> Commit()
        {
            try
            {
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

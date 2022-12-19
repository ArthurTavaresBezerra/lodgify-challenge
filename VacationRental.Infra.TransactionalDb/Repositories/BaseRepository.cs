using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Infra.TransactionalDb.Database;

namespace VacationRental.Infra.TransactionalDb.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : EntityBase
    {
        private TransDbContext Db;

        public BaseRepository(TransDbContext context)
        {
            Db = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync() => await Db.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity> GetByIdAsync(int id) => await Db.Set<TEntity>().FindAsync(id);

        public async Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = Db.Set<TEntity>().AsNoTracking().AsQueryable();

            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
                queryable = queryable.Include(includeProperty);

            return await queryable.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(TEntity obj ) => Db.Entry(obj).State = EntityState.Modified;

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            var entity = await Db.Set<TEntity>().AddAsync(obj);
            return entity.Entity;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : EntityBase
    {
        Task<TEntity> AddAsync(TEntity obj);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task UpdateAsync(TEntity obj);

    }
}

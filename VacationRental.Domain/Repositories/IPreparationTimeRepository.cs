using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IPreparationTimeRepository : IBaseRepository<PreparationTimeEntity>
    {
        IEnumerable<PreparationTimeEntity> GetAllByRental(int id);
        bool IsExistsByUnits(int rentalId, IEnumerable<int> unitsToRemove);
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Infra.TransactionalDb.Database;

namespace VacationRental.Infra.TransactionalDb.Repositories
{
    public class PreparationTimeRepository : BaseRepository<PreparationTimeEntity>, IPreparationTimeRepository
    {
        public PreparationTimeRepository(TransDbContext context) : base(context) { }

        public IEnumerable<PreparationTimeEntity> GetAllByRental(int id)
        {
            return Db.PreparationTimes.AsNoTracking().Include(c=> c.Booking).Where(c => c.Rental.Id == id).ToList();
        }

        public bool IsExistsByUnits(int rentalId, IEnumerable<int> unitsToRemove)
        {
            return Db.PreparationTimes.Where(c => c.Rental.Id == rentalId && unitsToRemove.Any(u => u == c.Unit)).Any();
        }
    }
}

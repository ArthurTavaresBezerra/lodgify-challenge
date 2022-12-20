using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.ViewModels;
using VacationRental.Infra.TransactionalDb.Database;

namespace VacationRental.Infra.TransactionalDb.Repositories
{
    public class BookingRepository : BaseRepository<BookingEntity>, IBookingRepository
    {

        public BookingRepository(TransDbContext context) : base(context) { }

        public IEnumerable<BookingEntity> GetAllByRental(int id)
        {
            return Db.Bookings.AsNoTracking().Where(c => c.Rental.Id == id).ToList();
        }

        public bool IsExistsByUnits(int rentalId, IEnumerable<int> unitsToRemove)
        {
            return Db.Bookings.Where(c => c.Rental.Id == rentalId && unitsToRemove.Any(u=> u == c.Unit)).Any();
        }
    }
}

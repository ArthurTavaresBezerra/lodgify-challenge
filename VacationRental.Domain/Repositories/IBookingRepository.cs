using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository : IBaseRepository<BookingEntity>
    {
        IEnumerable<BookingEntity> GetAllByRental(int id);
        bool IsExistsByUnits(int rentalId, IEnumerable<int> unitsToRemove);
    }
}

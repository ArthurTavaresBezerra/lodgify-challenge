using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Repositories
{
    public interface IBookingRepository : IBaseRepository<BookingEntity>
    {
    }
}

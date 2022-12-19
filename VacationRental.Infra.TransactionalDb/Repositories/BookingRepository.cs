using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Infra.TransactionalDb.Database;

namespace VacationRental.Infra.TransactionalDb.Repositories
{
    public class BookingRepository : BaseRepository<BookingEntity>, IBookingRepository
    {

        public BookingRepository(TransDbContext context) : base(context) { }
    }
}

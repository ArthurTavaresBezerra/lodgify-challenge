using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Infra.TransactionalDb.Database;

namespace VacationRental.Infra.TransactionalDb.Repositories
{
    public class RentalRepository : BaseRepository<RentalEntity>, IRentalRepository
    {

        public RentalRepository(TransDbContext context) : base(context) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.Entities;

namespace VacationRental.Infra.TransactionalDb.Database
{
    public class DatabaseManager 
    {
        private TransDbContext _context;
         
         
        public DatabaseManager(TransDbContext context)
        {
            _context = context;
        }

        public async Task EnsureCreated()
        {
            _context.Database.EnsureCreated();
        }
    }
}

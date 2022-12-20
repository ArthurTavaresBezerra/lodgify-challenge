using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain;

namespace VacationRental.Infra.TransactionalDb.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransDbContext _context;

        private bool _disposed;

        public UnitOfWork(TransDbContext context)
        {
            _context = context;
        }

        public void BeginTransaction()
        {
            _disposed = false;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

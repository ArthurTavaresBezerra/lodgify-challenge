using System;

namespace VacationRental.Domain
{
    public interface IUnitOfWork
    {
        void BeginTransaction();

        void Commit();

        void Dispose();

    }
}

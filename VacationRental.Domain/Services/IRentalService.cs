using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Domain.Services
{
    public interface IRentalService
    {
        Task<RentalEntity> GetById(int id);
    }
}

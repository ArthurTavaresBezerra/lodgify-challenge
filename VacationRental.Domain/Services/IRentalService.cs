using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Request.Rental;
using VacationRental.Domain.ViewModels.Response.Rental;

namespace VacationRental.Domain.Services
{
    public interface IRentalService
    {
        Task<GetRentalViewModel> GetById(int id);
        Task<GetRentalViewModel> Insert(PostRentalViewModel rentalViewModel);
        Task<GetRentalViewModel> Update(PutRentalViewModel rentalViewModel);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Domain.Services
{
    public interface IBookingService
    {
        Task<BookingViewModel> GetById(int id);
        Task<BookingViewModel> Insert(BookingViewModel rentalViewModel);
    }
}

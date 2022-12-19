using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels.Response.Calendar;

namespace VacationRental.Domain.Services
{
    public interface ICalendarService
    {
        Task<GetCalendarViewModel> GetCalendar(int rentalId, DateTime Start, int nights);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;
using System.Linq;

namespace VacationRental.BusinessLogic.Services
{
    public class CalendarService : ICalendarService
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<CalendarViewModel> GetCalendar(int rentalId, DateTime start, int nights)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookeds = _bookingRepository.GetAllByRental(rental.Id);

            var result = new CalendarViewModel
            {
                RentalId = rentalId,
                Dates = new List<CalendarDateViewModel>()
            };

            for (var night = 0; night < nights; night++)
            {
                DateTime currentDate = start.Date.AddDays(night).Date;

                var bookings = bookeds.Where(c => c.Start <= currentDate && c.End > currentDate)
                                        .Select(c=> new CalendarBookingViewModel { Id = c.Id })
                                        .ToList();

                result.Dates.Add( new CalendarDateViewModel
                {
                    Date = currentDate,
                    Bookings = bookings
                });
            }

            return result;
        }
    }
}

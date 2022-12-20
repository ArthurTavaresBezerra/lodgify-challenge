using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels.Response.Calendar;
using System.Linq;

namespace VacationRental.BusinessLogic.Services
{
    public class CalendarService : ICalendarService
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IPreparationTimeRepository _preparationTimeRepository;

        public CalendarService(
                IBookingRepository bookingRepository, 
                IRentalRepository rentalRepository,
                IPreparationTimeRepository preparationTimeRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _preparationTimeRepository = preparationTimeRepository;
        }

        public async Task<GetCalendarViewModel> GetCalendar(int rentalId, DateTime start, int nights)
        {
            var rental = await _rentalRepository.GetByIdAsync(rentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookeds = _bookingRepository.GetAllByRental(rental.Id);
            var ppTimes = _preparationTimeRepository.GetAllByRental(rental.Id);


            return MountObjectResult( rental, start, nights, bookeds, ppTimes);
        }

        private GetCalendarViewModel MountObjectResult(
                RentalEntity rental,
                DateTime start, 
                int nights,
                IEnumerable<BookingEntity> bookeds, 
                IEnumerable<PreparationTimeEntity> ppTimes)
        {

            List<GetCalendarDateViewModel> dates = new List<GetCalendarDateViewModel>();

            foreach (DateTime currentDate in DateRange(start, nights))
                dates.Add(MountCalendarDateView(currentDate, bookeds, ppTimes));

            return new GetCalendarViewModel { RentalId = rental.Id, Dates = dates };
        }

        private GetCalendarDateViewModel MountCalendarDateView(
            DateTime currentDate,
            IEnumerable<BookingEntity> bookeds,
            IEnumerable<PreparationTimeEntity> ppTimes)
        {
            var bookingsCal = bookeds.Where(c => c.Start <= currentDate && c.End > currentDate)
                                          .Select(c => new GetCalendarBookingViewModel { Id = c.Id, Unit = c.Unit })
                                          .ToList();

            var ppTimesCal = ppTimes.Where(c => c.Start <= currentDate && c.End > currentDate)
                                    .Select(c => new GetCalendarPreparationTimeViewModel { Unit = c.Unit })
                                    .ToList();
            
            return new GetCalendarDateViewModel
            {
                Date = currentDate,
                Bookings = bookingsCal,
                PreparationTimes = ppTimesCal
            };
        }

        private List<DateTime> DateRange(DateTime start, int nights)
        {
            List<DateTime> dateRange = new List<DateTime>();
            for (var night = 0; night < nights; night++)
            {
                dateRange.Add(start.Date.AddDays(night).Date);
            }
            return dateRange;
        }
    }
}

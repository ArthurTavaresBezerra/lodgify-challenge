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
    public class AvailabilityCheckService : IAvailabilityCheckService
    {

        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public AvailabilityCheckService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task Check(BookingViewModel newBooking)
        {
            var rental = await _rentalRepository.GetByIdAsync(newBooking.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            var bookeds = _bookingRepository.GetAllByRental(rental.Id);

            Func<BookingEntity, bool> isAlredyBooked = (booked) =>
            {

                var isStartIn = booked.Start <= newBooking.Start.Date && booked.End > newBooking.Start.Date;
                var isEndIn = booked.Start < newBooking.End && booked.End >= newBooking.End;
                var isBookedIntoNewBooking = booked.Start > newBooking.Start && booked.End < newBooking.End;

                return (isStartIn || isEndIn || isBookedIntoNewBooking);
            };

            var unitsInUse = bookeds.Where(isAlredyBooked).Count();
            if (unitsInUse >= rental.Units)
                throw new ApplicationException("Not available");
        }
    }
}

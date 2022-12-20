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
        private readonly IPreparationTimeRepository _preparationTimeRepository;


        public AvailabilityCheckService(
                IBookingRepository bookingRepository, 
                IPreparationTimeRepository preparationTimeRepository)
        {
            _bookingRepository = bookingRepository;
            _preparationTimeRepository = preparationTimeRepository;
        }

        public IEnumerable<AppointmentEntityBase> CheckAvailability(
                RentalEntity rental,
                AppointmentEntityBase newAppointment, 
                bool withPreparationTimes = true)
        {

            List<AppointmentEntityBase> unitsBookedOrBlockeds = UnitsBookeds(newAppointment, rental.Id).ToList();

            if (withPreparationTimes)
                unitsBookedOrBlockeds.AddRange(UnitsBlockeds(newAppointment, rental.Id).ToList());

            bool thereArentAnyUnitAvailable = unitsBookedOrBlockeds.GroupBy(c => c.Unit).Count() >= rental.Units;
            bool thatUnitIsNotAvailable = newAppointment.Unit > 0 && unitsBookedOrBlockeds.Any(c => c.Unit == newAppointment.Unit);

            if (thereArentAnyUnitAvailable || thatUnitIsNotAvailable) 
                throw new ApplicationException("Not available");

            return unitsBookedOrBlockeds;
        }

        private IEnumerable<AppointmentEntityBase> UnitsBookeds(AppointmentEntityBase newAppointment, int rentalId)
        {
            var bookeds = _bookingRepository.GetAllByRental(rentalId);
            return bookeds.Where(c => isAlredyBookedFunction(c, newAppointment)).ToList();
        }
        
        private IEnumerable<AppointmentEntityBase> UnitsBlockeds(AppointmentEntityBase newAppointment, int rentalId)
        {
            var preparationtimes = _preparationTimeRepository.GetAllByRental(rentalId);
            return preparationtimes.Where(pt => isAlredyBookedFunction(pt, newAppointment)).ToList();
        }

        private Func<AppointmentEntityBase, AppointmentEntityBase, bool> isAlredyBookedFunction = (booked, newAppointment) =>
        {
            var isStartIn = booked.Start <= newAppointment.Start.Date && booked.End > newAppointment.Start.Date;
            var isEndIn = booked.Start < newAppointment.End && booked.End >= newAppointment.End;
            var isBookedIntoNewBooking = booked.Start > newAppointment.Start && booked.End < newAppointment.End;

            return (isStartIn || isEndIn || isBookedIntoNewBooking);
        };
    }
}

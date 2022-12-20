using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;

namespace VacationRental.BusinessLogic.Services
{
    public class BookingService : IBookingService
    {
        private readonly IAvailabilityCheckService _availabilityCheckService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IPreparationTimeRepository _preparationTimeRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(
                IAvailabilityCheckService availabilityCheckService, 
                IBookingRepository bookingRepository,
                IRentalRepository rentalRepository,
                IPreparationTimeRepository preparationTimeRepository,
                IMapper mapper, 
                IUnitOfWork unitOfWork)
        {
            _availabilityCheckService = availabilityCheckService;
            _bookingRepository = bookingRepository;
            _rentalRepository = rentalRepository;
            _preparationTimeRepository = preparationTimeRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingViewModel> Insert(BookingViewModel newBooking)
        {
            var rental = await _rentalRepository.GetByIdAsync(newBooking.RentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            IEnumerable<AppointmentEntityBase> unitsInUseOrBlocked = CheckAvailabilityToABooking(rental, _mapper.Map<BookingEntity>(newBooking));

            newBooking.Unit = GetNumberOfAvailableUnit(rental.Units, unitsInUseOrBlocked);

            _unitOfWork.BeginTransaction();
            var storagedBooking = await _bookingRepository.AddAsync(_mapper.Map<BookingEntity>(newBooking));
            if (rental.PreparationTimeInDays > 0)
            {
                await _preparationTimeRepository.AddAsync(mountPreparationTime(rental, storagedBooking));
            }
            _unitOfWork.Commit();
            return _mapper.Map<BookingViewModel>(storagedBooking);
        }
      

        public async Task<BookingViewModel> GetById(int id)
        {
            var rental = await _bookingRepository.GetByIdAsync(id, (a) => a.Rental);

            if (rental == null)
                throw new ApplicationException("Booking not found");

            return _mapper.Map<BookingEntity, BookingViewModel>(rental);
        }
        
        private int GetNumberOfAvailableUnit(int NumberOfUnits, IEnumerable<AppointmentEntityBase> unitsInUseOrBlocked)
        {
            var listOfUnits = Enumerable.Range(1, NumberOfUnits);

            var unitAvailable = listOfUnits.Where(unit => !unitsInUseOrBlocked.Any(a => a.Unit == unit)).FirstOrDefault();

            if (unitAvailable > 0)
                return unitAvailable;

            throw new ApplicationException("Any unit available");
        }

        private IEnumerable<AppointmentEntityBase> CheckAvailabilityToABooking(RentalEntity rental, AppointmentEntityBase appointment)
        {
            if (rental.PreparationTimeInDays > 0)
                appointment.End = appointment.End.AddDays(rental.PreparationTimeInDays);
            return _availabilityCheckService.CheckAvailability(rental, appointment);
        }

        private PreparationTimeEntity mountPreparationTime(RentalEntity rental, BookingEntity booking)
        {
            return new PreparationTimeEntity() 
            { 
                Rental = rental, 
                Unit = booking.Unit, 
                Start = booking.End, 
                End = booking.End.AddDays(rental.PreparationTimeInDays),
                Booking = booking
            };
        }
    }
}

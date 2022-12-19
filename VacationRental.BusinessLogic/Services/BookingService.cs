using AutoMapper;
using System;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IAvailabilityCheckService availabilityCheckService, IBookingRepository bookingRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _availabilityCheckService = availabilityCheckService;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        } 
         
        public async Task<BookingViewModel> Insert(BookingViewModel newBooking)
        {
            await _availabilityCheckService.Check(newBooking);
            _unitOfWork.BeginTransaction();
            var storagedBooking = await _bookingRepository.AddAsync(_mapper.Map<BookingEntity>(newBooking));
            _unitOfWork.Commit();
            return _mapper.Map<BookingViewModel>(storagedBooking);
        }

        public async Task<BookingViewModel> GetById(int id)
        {
            var rental = await _bookingRepository.GetByIdAsync(id);

            if (rental == null)
                throw new ApplicationException("Booking not found");

            return _mapper.Map<BookingEntity, BookingViewModel>(rental);
        }
    }
}

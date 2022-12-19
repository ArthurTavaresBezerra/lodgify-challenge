using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Request.Rental;
using VacationRental.Domain.ViewModels.Response.Rental;

namespace VacationRental.BusinessLogic.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPreparationTimeRepository _preparationTimeRepository;
        private readonly IAvailabilityCheckService _availabilityCheckService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RentalService(
                IRentalRepository rentalRepository,
                IBookingRepository bookingRepository,
                IPreparationTimeRepository preparationTimeRepository,
                IAvailabilityCheckService availabilityCheckService,
                IMapper mapper, 
                IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _bookingRepository = bookingRepository;
            _preparationTimeRepository = preparationTimeRepository;
            _availabilityCheckService = availabilityCheckService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<GetRentalViewModel> GetById(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            return _mapper.Map<RentalEntity, GetRentalViewModel>(rental);
        }

        public async Task<GetRentalViewModel> Insert(PostRentalViewModel rentalViewModel)
        {
            _unitOfWork.BeginTransaction();
            var storagedRental = await _rentalRepository.AddAsync(_mapper.Map<RentalEntity>(rentalViewModel));
            _unitOfWork.Commit();
            return _mapper.Map<GetRentalViewModel>(storagedRental);
        }

        public async Task<GetRentalViewModel> Update(PutRentalViewModel putRental)
        {
            var rental = await _rentalRepository.GetByIdAsync(putRental.Id);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            checkUnitOverlapping(rental, putRental);
            checkPreparationTimeOverlapping(rental, putRental);

            
            _unitOfWork.BeginTransaction();

            rental.Units = putRental.Units;
            rental.PreparationTimeInDays = putRental.PreparationTimeInDays;
            
            _rentalRepository.UpdateAsync(rental);
            await RenewPreparationTimes(rental, putRental);
            _unitOfWork.Commit();
            return _mapper.Map<GetRentalViewModel>(rental);          
        }

        public async Task RenewPreparationTimes(RentalEntity rental, PutRentalViewModel putRental)
        {
            var allPpTimesToRemove = _preparationTimeRepository.GetAllByRental(rental.Id);

            var ppTimesToInsert = allPpTimesToRemove.Select( pt => new PreparationTimeEntity()
            {
                Rental = rental,
                Unit = pt.Unit,
                Start = pt.Start,
                End = pt.Start.AddDays(putRental.PreparationTimeInDays),
                Booking = pt.Booking
            }).ToList();

            _unitOfWork.BeginTransaction();

            _preparationTimeRepository.RemoveRangeAsync(allPpTimesToRemove);
            if (rental.PreparationTimeInDays > 0)
            {
                await _preparationTimeRepository.AddRangeAsync(ppTimesToInsert);
            }
            _unitOfWork.Commit();
        }

        private void checkUnitOverlapping(RentalEntity rental, PutRentalViewModel putRental)
        {
            if (putRental.Units < rental.Units) 
            {
                IEnumerable<int> unitsToRemove = Enumerable.Range(putRental.Units+1, rental.Units - putRental.Units);
                bool hasBookings = _bookingRepository.IsExistsByUnits(rental.Id, unitsToRemove);
                bool hasPreparations = _preparationTimeRepository.IsExistsByUnits(rental.Id, unitsToRemove);

                if (hasBookings || hasPreparations)
                {
                    throw new ApplicationException("Overlapping on decreasing number of Units");
                }
            }
        }

        private void checkPreparationTimeOverlapping(RentalEntity rental, PutRentalViewModel putRental)
        {
            if (putRental.PreparationTimeInDays > rental.PreparationTimeInDays)
            {
                var pptTimes = _preparationTimeRepository.GetAllByRental(rental.Id);

                try
                {
                    foreach (var pptime in pptTimes)
                    {
                        pptime.End = pptime.Start.AddDays(putRental.PreparationTimeInDays);
                        _availabilityCheckService.CheckAvailability(rental, pptime, withPreparationTimes: false);
                    }
                }
                catch (ApplicationException)
                {
                    throw new ApplicationException("Overlapping on increasing preparation times");
                }
            }
        }
    }
}

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
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public RentalService(IRentalRepository rentalRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RentalViewModel> GetById(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            return _mapper.Map<RentalEntity, RentalViewModel>(rental);
        }

        public async Task<RentalViewModel> Insert(RentalViewModel rentalViewModel)
        {
            _unitOfWork.BeginTransaction();
            var storagedRental = await _rentalRepository.AddAsync(_mapper.Map<RentalEntity>(rentalViewModel));
            _unitOfWork.Commit();
            return _mapper.Map<RentalViewModel>(storagedRental);
        }
    }
}

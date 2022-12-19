using AutoMapper;
using System;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;

namespace VacationRental.BusinessLogic
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IMapper _mapper;

        public RentalService(IRentalRepository rentalRepository, IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _mapper = mapper;
        }

        public async Task<RentalViewModel> GetById(int id)
        {
            var rental = await _rentalRepository.GetByIdAsync(id);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            return _mapper.Map<RentalEntity, RentalViewModel>(rental);
        }
    }
}

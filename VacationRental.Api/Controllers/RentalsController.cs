using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.Models;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IDictionary<int, RentalViewModel> _rentals;
        private readonly IRentalService _rentalService;

        public RentalsController(IDictionary<int, RentalViewModel> rentals, IRentalService rentalService)
        {
            _rentals = rentals;
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<RentalViewModel> Get(int rentalId)
        {
            var rental = await _rentalService.GetById(rentalId);

            if (rental == null)
                throw new ApplicationException("Rental not found");

            return rental;
        }

        [HttpPost]
        public ResourceIdViewModel Post(RentalBindingModel model)
        {
            var key = new ResourceIdViewModel { Id = _rentals.Keys.Count + 1 };

            _rentals.Add(key.Id, new RentalViewModel
            {
                Id = key.Id,
                Units = model.Units
            });

            return key;
        }
    }
}

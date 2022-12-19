using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<ActionResult<RentalViewModel>> Get(int rentalId) => Ok(await _rentalService.GetById(rentalId));

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(RentalBindingModel model)
        {
            RentalViewModel rental = await _rentalService.Insert( new RentalViewModel { Units = model.Units });
            return Ok(new ResourceIdViewModel { Id = rental.Id});
        }
    }
}

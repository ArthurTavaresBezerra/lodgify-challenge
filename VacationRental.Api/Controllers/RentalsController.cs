using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Request.Rental;
using VacationRental.Domain.ViewModels.Response.Rental;

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
        public async Task<ActionResult<GetRentalViewModel>> Get(int rentalId) => Ok(await _rentalService.GetById(rentalId));

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(PostRentalBindingModel model)
        {
            GetRentalViewModel rental = await _rentalService.Insert(new PostRentalViewModel 
            { 
                Units = model.Units, 
                PreparationTimeInDays = model.PreparationTimeInDays 
            });
            return Ok(new ResourceIdViewModel { Id = rental.Id });
        }

        [HttpPut]
        public async Task<ActionResult<ResourceIdViewModel>> Put(PutRentalBindingModel model)
        {
            GetRentalViewModel rental = await _rentalService.Update(new PutRentalViewModel 
            { 
                Id = model.Id, 
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            });
            return Ok(new ResourceIdViewModel { Id = rental.Id });
        }
    }
}

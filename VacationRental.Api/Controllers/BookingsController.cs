using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<ActionResult<BookingViewModel>> Get(int bookingId) => Ok(await _bookingService.GetById(bookingId));

        [HttpPost]
        public async Task<ActionResult<ResourceIdViewModel>> Post(BookingBindingModel model)
        {
            var booked = await _bookingService.Insert(new BookingViewModel { Start = model.Start, Nights = model.Nights, RentalId = model.RentalId });
            return Ok(new ResourceIdViewModel { Id = booked.Id });
        }
    }
}

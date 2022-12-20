using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Api.BindingModels;
using VacationRental.Domain.Services;
using VacationRental.Domain.ViewModels.Response.Calendar;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/calendar")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpGet]
        public async Task<ActionResult<GetCalendarViewModel>> Get([FromQuery] CalendarBindingModel calendar) => Ok(await _calendarService.GetCalendar(calendar.RentalId, calendar.Start, calendar.Nights));
    }
}

using System;
using System.Collections.Generic;

namespace VacationRental.Domain.ViewModels.Response.Calendar
{
    public class GetCalendarDateViewModel
    {
        public DateTime Date { get; set; }
        public List<GetCalendarBookingViewModel> Bookings { get; set; }
        public List<GetCalendarPreparationTimeViewModel> PreparationTimes { get; set; }
    }
}

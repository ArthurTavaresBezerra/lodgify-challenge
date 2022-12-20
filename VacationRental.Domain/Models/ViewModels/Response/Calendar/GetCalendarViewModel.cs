using System.Collections.Generic;

namespace VacationRental.Domain.ViewModels.Response.Calendar
{
    public class GetCalendarViewModel
    {
        public int RentalId { get; set; }
        public List<GetCalendarDateViewModel> Dates { get; set; }
    }
}

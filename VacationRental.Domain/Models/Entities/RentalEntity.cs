using System.Collections.Generic;

namespace VacationRental.Domain.Entities
{
    public class RentalEntity : EntityBase
    { 
        public int Units { get; set; }

        public IEnumerable<BookingEntity> Bookings { get; set; }
    }
}

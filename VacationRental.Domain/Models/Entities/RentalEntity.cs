using System.Collections.Generic;

namespace VacationRental.Domain.Entities
{
    public class RentalEntity : EntityBase
    { 
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        public IEnumerable<BookingEntity> Bookings { get; set; }
        public IEnumerable<PreparationTimeEntity> PreparationTimes { get; set; }
    }
}

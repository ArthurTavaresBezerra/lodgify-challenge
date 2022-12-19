using System;

namespace VacationRental.Domain.Entities
{
    public class BookingEntity : EntityBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public RentalEntity Rental { get; set; }
    }
}

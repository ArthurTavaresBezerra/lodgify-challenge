using System;
using System.Collections.Generic;

namespace VacationRental.Domain.Entities
{
    public class BookingEntity : AppointmentEntityBase
    {
        public RentalEntity Rental { get; set; }
        public IEnumerable<PreparationTimeEntity> PreparationTimes { get; set; }
    }
}

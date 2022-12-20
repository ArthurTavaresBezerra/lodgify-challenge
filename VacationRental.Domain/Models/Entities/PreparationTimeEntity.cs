using System;

namespace VacationRental.Domain.Entities
{
    public class PreparationTimeEntity : AppointmentEntityBase
    {
        public RentalEntity Rental { get; set; }
        public BookingEntity Booking { get; set; }
    }
}

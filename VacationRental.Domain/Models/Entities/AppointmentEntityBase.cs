using System;

namespace VacationRental.Domain.Entities
{
    public class AppointmentEntityBase: EntityBase
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Unit { get; set; }
    }
}

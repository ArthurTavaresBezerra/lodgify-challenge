using System;

namespace VacationRental.Domain.Entities
{
    public class EntityBase 
    {
        public int Id { get; set; }
        public DateTime CreateAt { get; set; }
    }
}

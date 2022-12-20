using System.ComponentModel.DataAnnotations;
using System;

namespace VacationRental.Api.BindingModels
{
    public class CalendarBindingModel
    {
        [Required]
        public int RentalId { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        [Range(0, 365)]
        public int Nights { get; set; }
    }
}

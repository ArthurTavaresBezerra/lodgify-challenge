using System;
using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.Models
{
    public class BookingBindingModel
    {

        [Required]
        public int RentalId { get; set; }

        [Required]
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;

        [Required]
        [Range(1,365)]
        public int Nights { get; set; }
    }
}

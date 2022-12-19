﻿using System;

namespace VacationRental.Domain.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public DateTime Start { get; set; }
        public int Nights { get; set; }
        public DateTime End => Start.AddDays(Nights);

    }
}

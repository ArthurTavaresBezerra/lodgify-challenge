﻿using System.ComponentModel.DataAnnotations;

namespace VacationRental.Api.BindingModels
{
    public class PutRentalBindingModel
    {
        [Required]        
        public int Id { get; set; }
        [Required]
        [Range(1, 99999999999)]
        public int Units { get; set; }
        [Required]
        public int PreparationTimeInDays { get; set; }
    }
}

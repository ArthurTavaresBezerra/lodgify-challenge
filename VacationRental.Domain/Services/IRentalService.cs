﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Domain.Services
{
    public interface IRentalService
    {
        Task<RentalViewModel> GetById(int id);
        Task<RentalViewModel> Insert(RentalViewModel rentalViewModel);
    }
}

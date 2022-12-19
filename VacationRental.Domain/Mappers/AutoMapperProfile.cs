using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;

namespace VacationRental.Domain.Mappers
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            this.CreateMap<RentalEntity, RentalViewModel>();
        }
    }
}

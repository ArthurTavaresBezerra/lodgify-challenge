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
            this.CreateMap<RentalViewModel, RentalEntity>();

            this.CreateMap<BookingViewModel, BookingEntity>()
                .ForMember(d=> d.End, s=> s.MapFrom(m=> m.Start.AddDays(m.Nights)))
                .ForMember(d=> d.Rental, s=> s.MapFrom(m=> new RentalEntity { Id = m.RentalId }));

            this.CreateMap<BookingEntity, BookingViewModel>()
                .ForMember(d => d.Nights, s => s.MapFrom(m => m.End.Subtract(m.Start).TotalDays))
                .ForMember(d => d.RentalId, s => s.MapFrom(m => m.Rental.Id));
            ;
        }
    }
}

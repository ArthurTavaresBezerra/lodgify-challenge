using AutoMapper;
using VacationRental.Domain.Entities;
using VacationRental.Domain.ViewModels;
using VacationRental.Domain.ViewModels.Request.Rental;
using VacationRental.Domain.ViewModels.Response.Rental;

namespace VacationRental.Domain.Mappers
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            this.CreateMap<RentalEntity, PutRentalViewModel>();
            this.CreateMap<PutRentalViewModel, RentalEntity>();
            this.CreateMap<RentalEntity, PostRentalViewModel>();
            this.CreateMap<PostRentalViewModel, RentalEntity>();
            this.CreateMap<RentalEntity, GetRentalViewModel>();
            this.CreateMap<GetRentalViewModel, RentalEntity>();

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

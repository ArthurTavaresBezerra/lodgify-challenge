using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Infra.TransactionalDb.Database;
using VacationRental.Domain;
using VacationRental.Domain.Repositories;
using VacationRental.Domain.Mappers;
using VacationRental.Domain.Services;
using VacationRental.Infra.TransactionalDb.Repositories;
using AutoMapper;
using VacationRental.BusinessLogic.Services;
using Microsoft.AspNetCore.Hosting;

namespace VacationRental.CrossCutting
{
    public class NativeInjector
    {
        private static string _connection = "DefaultConnection";
        private static string _testEnvironment = "test";

        public static void InjectContext(IServiceCollection services, IConfiguration configuration)
        {
            var env = GetEnvironmentInstance(services);
            if (env.EnvironmentName == _testEnvironment)
            {
                services.AddDbContext<TransDbContext>(options => options.UseInMemoryDatabase(databaseName: "DbInMemory"));
            }
            else
            {
                var databaseConnectionString = configuration.GetConnectionString(_connection);
                services.AddDbContext<TransDbContext>(
                    optionsBuilder => optionsBuilder.UseSqlServer(databaseConnectionString));
                services.AddScoped<DatabaseManager, DatabaseManager>();
            }
        }

        public static void RegisterServices(IServiceCollection services)
        {
            //Business
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IAvailabilityCheckService, AvailabilityCheckService>();
            services.AddScoped<ICalendarService, CalendarService>();
            //Repositories
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IPreparationTimeRepository, PreparationTimeRepository>();
            //Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void RegisterAutoMapper(IServiceCollection services)
        {
            MapperConfiguration mc = new MapperConfiguration(c=> 
            {
                c.AddProfile(new AutoMapperProfile());
            });
            services.AddSingleton(mc.CreateMapper());
        }

        public static void ConfigureContext(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                var databaseManager = scope?.ServiceProvider.GetService<DatabaseManager>();

                if (databaseManager != null)
                    _ = databaseManager.EnsureCreated();
            }
        }

        public static IHostingEnvironment GetEnvironmentInstance(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                return scope?.ServiceProvider.GetService<IHostingEnvironment>();
            }
        }

    }
}

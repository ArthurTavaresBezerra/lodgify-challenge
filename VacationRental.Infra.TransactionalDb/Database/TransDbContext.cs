
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Domain.Entities;

namespace VacationRental.Infra.TransactionalDb.Database
{
    public class TransDbContext : DbContext
    {
        public TransDbContext(DbContextOptions<TransDbContext> options) : base(options)
        {  
        }

        public DbSet<BookingEntity> Bookings { get; set; }

        public DbSet<RentalEntity> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<RentalEntity>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            //modelBuilder.Entity<Url>();

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            RentalEntity rental = new RentalEntity { Id = 1, CreateAt = DateTime.UtcNow, Units = 3, Bookings = new List<BookingEntity>() };
            modelBuilder.Entity<RentalEntity>().HasData(rental);


            modelBuilder.Entity<BookingEntity>().OwnsOne(p => p.Rental).HasData(
                new BookingEntity() { Id = 1, Start = DateTime.Parse("2022-12-01"), End = DateTime.Parse("2022-12-03"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 2, Start = DateTime.Parse("2022-12-01"), End = DateTime.Parse("2022-12-04"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 3, Start = DateTime.Parse("2022-12-04"), End = DateTime.Parse("2022-12-06"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 4, Start = DateTime.Parse("2022-12-06"), End = DateTime.Parse("2022-12-06"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 5, Start = DateTime.Parse("2022-12-12"), End = DateTime.Parse("2022-12-18"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 6, Start = DateTime.Parse("2022-12-20"), End = DateTime.Parse("2022-12-22"), CreateAt = DateTime.UtcNow },
                new BookingEntity() { Id = 7, Start = DateTime.Parse("2022-12-24"), End = DateTime.Parse("2022-12-25"), CreateAt = DateTime.UtcNow }
            );
        }
    }
}
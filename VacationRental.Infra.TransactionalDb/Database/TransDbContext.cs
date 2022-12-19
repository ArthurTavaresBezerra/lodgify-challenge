
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

            modelBuilder.Entity<BookingEntity>()
                .HasOne(c => c.Rental)
                .WithMany(c => c.Bookings)
                .IsRequired();

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
        }
    }
}
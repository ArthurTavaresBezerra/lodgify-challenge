
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
        
        public DbSet<PreparationTimeEntity> PreparationTimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingEntity>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<BookingEntity>()
                .HasOne(c => c.Rental)
                .WithMany(c => c.Bookings)
                .IsRequired();

            modelBuilder.Entity<PreparationTimeEntity>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<PreparationTimeEntity>()
                            .HasOne(c => c.Booking)
                            .WithMany(c => c.PreparationTimes)
                            .IsRequired();

            modelBuilder.Entity<PreparationTimeEntity>()
                .HasOne(c => c.Rental)
                .WithMany(c => c.PreparationTimes)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<RentalEntity>()
            .Property(f => f.Id)
            .ValueGeneratedOnAdd();

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            RentalEntity rental = new RentalEntity { Id = 1, CreateAt = DateTime.UtcNow, Units = 3, PreparationTimeInDays = 1, Bookings = new List<BookingEntity>() };
            modelBuilder.Entity<RentalEntity>().HasData(rental);
        }
    }
}
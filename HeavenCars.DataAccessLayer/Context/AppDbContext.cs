using HeavenCars.DataAccessLayer.Models;
using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Models.Cars;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeavenCars.DataAccesLayer.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<BookingVehicule> BookingVehicules { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>()
                .HasOne<ApplicationUser>(a => a.Verzender)
                .WithMany(d => d.Messages)
                .HasForeignKey(d => d.UserID);

            modelBuilder.Seed();
        }
    }
}

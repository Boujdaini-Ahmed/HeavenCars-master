using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeavenCars.DataAccessLayer.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(AppDbContext context, ILogger<BookingRepository> logger)
        {
            this.context = context;
            _logger = logger;
        }

        public BookingVehicule AddBooking(BookingVehicule booking)
        {
            var newBooking = context.BookingVehicules.Add(booking);

            if (newBooking != null && newBooking.State == EntityState.Added)
            {
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    return newBooking.Entity;
                }
            }

            return null;
        }

        public BookingVehicule Create(BookingVehicule nouveaubookingquidoitallerdansbanquededonnees)
        {
            {
                try
                {
                    if (nouveaubookingquidoitallerdansbanquededonnees != null)
                    {
                        var newBookingEntityEntry = context.BookingVehicules.Add(nouveaubookingquidoitallerdansbanquededonnees);

                        if (newBookingEntityEntry != null &&
                            newBookingEntityEntry.State == EntityState.Added)
                        {
                            var affectedRows = context.SaveChanges();

                            if (affectedRows > 0)
                            {
                                _logger.LogInformation($"The booking {nouveaubookingquidoitallerdansbanquededonnees.BookingId} is created.");
                                return newBookingEntityEntry.Entity;
                            }
                        }
                    }

                    _logger.LogWarning($"The booking given is Null.");
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"When creating a booking Failed.");
                    throw;
                }
            }
        }

        public IEnumerable<BookingVehicule> GetAllBookings()
        {
            var listBookings = context.BookingVehicules.Include(x => x.ApplicationUser);
            return listBookings;
            //return context.BookingVehicules;
        }

        public BookingVehicule GetBooking(int GetBookingId)
        {
            var booking = context.BookingVehicules
               .Include(x => x.ApplicationUser)
               .Where(x => x.BookingId == GetBookingId).FirstOrDefault();

            return booking;
        }

        public BookingVehicule GetBooking(int? BookingId)
        {
            var booking = context.BookingVehicules
               .Include(x => x.ApplicationUser)
               .Where(x => x.BookingId == BookingId).FirstOrDefault();

            return booking;
        }

        public async Task<BookingVehicule> GetByIdAsync(int id)
        {
            return await context.BookingVehicules.FindAsync(id);
        }
    }
}

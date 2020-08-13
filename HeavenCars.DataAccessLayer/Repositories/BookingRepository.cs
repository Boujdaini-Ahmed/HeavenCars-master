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
            //context.BookingVehicules.Add(nouveaubookingquidoitallerdansbanquededonnees);
            //context.SaveChanges();
            //return nouveaubookingquidoitallerdansbanquededonnees;
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

        public BookingVehicule Delete(BookingVehicule bookingVehicule)
        {

            //Car carToBeDeleted = context.Cars.Find(car.CarId);
            var bookingetresupprimer = context.BookingVehicules.Remove(bookingVehicule);

            if (bookingetresupprimer != null && bookingetresupprimer.State == EntityState.Deleted)
            {
                //context.Cars.Update(carToBeDeleted);
                //context.SaveChanges();
                var affectedRows = context.SaveChanges();

                if (affectedRows > 0)
                {
                    _logger.LogInformation($"The {bookingVehicule.BookingId} was deleted.");
                    return bookingetresupprimer.Entity;
                }
            }
            return null;
            //return carToBeDeleted;
        }

        public BookingVehicule FindBooking(int FindId)
        {
            var findbooking = context.BookingVehicules.Find(FindId);
            return findbooking;
        }

        public IEnumerable<BookingVehicule> GetAllBookings()
        {
            var listBookings = context.BookingVehicules.Include(x => x.ApplicationUser).ThenInclude(x => x.Car).ToList();

            return listBookings;
            //return context.BookingVehicules;
        }

     

        public BookingVehicule GetBooking(int id)
        {

            //var booking = context.BookingVehicules.Find(id);
            //return booking;
            var booking = context.BookingVehicules
               .Include(x => x.ApplicationUser)
               .Include(x => x.Car).ThenInclude(x => x.CarModel).ThenInclude(x => x.Brand)
                .Where(x => x.BookingId == id).ToList()
               .FirstOrDefault();

            return booking;
        }

  
        public BookingVehicule Update(BookingVehicule booking)
        {
            var bookingupdate = context.BookingVehicules.Attach(booking);
            bookingupdate.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return booking;
            //try
            //{
            //    if (booking != null)
            //    {
            //        var updateBookingEntityEntry = context.BookingVehicules.Update(booking);

            //        if (updateBookingEntityEntry != null &&
            //            updateBookingEntityEntry.State == EntityState.Modified)
            //        {
            //            var affectedRows = context.SaveChanges();

            //            if (affectedRows > 0)
            //            {
            //                _logger.LogInformation($"The {booking.BookingId} was updated.");
            //                return updateBookingEntityEntry.Entity;
            //            }
            //        }
            //    }
            //    _logger.LogInformation($"The booking given as null.");
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, $"When updating a booking Failed.");
            //    throw;
            //}
        }
    }
    }


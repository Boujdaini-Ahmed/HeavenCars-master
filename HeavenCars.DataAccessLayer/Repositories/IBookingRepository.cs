using HeavenCars.DataAccessLayer.Models.Bookings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeavenCars.DataAccessLayer.Repositories
{
    public interface IBookingRepository
    {
        BookingVehicule GetBooking(int id);
        IEnumerable<BookingVehicule> GetAllBookings();

        Task<BookingVehicule> GetByIdAsync(int id);

        BookingVehicule Create(BookingVehicule bookingVehicule);

        BookingVehicule AddBooking(BookingVehicule booking);

        BookingVehicule Update(BookingVehicule booking);
    }
}

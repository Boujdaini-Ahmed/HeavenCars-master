using HeavenCars.DataAccessLayer.Models.Bookings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HeavenCars.DataAccessLayer.Repositories
{
    public interface IBookingRepository
    {
        BookingVehicule GetBooking(int Id);
        IEnumerable<BookingVehicule> GetAllBookings();

        BookingVehicule Create(BookingVehicule bookingVehicule);

        BookingVehicule AddBooking(BookingVehicule booking);

        BookingVehicule Update(BookingVehicule booking);

        BookingVehicule FindBooking(int FindId);

        BookingVehicule Delete(BookingVehicule bookingVehicule);


    }
}

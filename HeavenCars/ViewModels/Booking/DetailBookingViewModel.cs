using HeavenCars.DataAccessLayer.Models.Bookings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Booking
{
    public class DetailBookingViewModel
    {
        public BookingVehicule BookingVehicule { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public bool IsSelected { get; set; }

    }
}


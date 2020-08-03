using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Booking
{
    public class DetailBookingViewModel : CreateBookingViewModel
    {
        public BookingVehicule BookingVehicule { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public bool IsSelected { get; set; }

  

    }
}


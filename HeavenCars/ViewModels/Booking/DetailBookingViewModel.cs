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
        public string Days { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }

        public bool IsSelected { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Mobile { get; set; }
        public string Comment { get; set; }
        public double TotalOrder { get; set; }
        public int OrderId { get; set; }
        public string Token { get; set; }
        public int VehicleId { get; set; }


    }
}


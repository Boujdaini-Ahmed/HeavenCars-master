using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Models.Cars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Booking
{
    public class DetailBookingViewModel 
    {
        public int BookingId { get; set; }
        public string BrandName { get; set; }
        public  string ModelName { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public  DateTime EndDate { get; set; }
        public BookingStatus BookingVanStatus { get; set; }
        public  int Prijs { get; set; }
        public string Days { get; set; }
        public string Total { get; set; }
        public string ExistingPhotoCar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber{ get; set; }
        public  ApplicationUser ApplicationUser { get; set; }


  


    }
}


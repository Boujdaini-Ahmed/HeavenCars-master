using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Cars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Bookings
{
   public class BookingVehicule
    {
        [Key]
        public int BookingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CreateBy { get; set; }

        //[ForeignKey("BookingStatus")]

        //public int BookingStatusId { get; set; }
        //public virtual BookingStatus BookingStatus { get; set; }

        [ForeignKey("Car")]

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}

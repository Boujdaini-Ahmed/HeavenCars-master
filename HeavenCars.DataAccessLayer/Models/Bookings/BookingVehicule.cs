﻿using HeavenCars.DataAccessLayer.Models.Account;
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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is mandatory")]
  
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is mandatory")]
        
        public DateTime EndDate { get; set; }

        public string CreateBy { get; set; }
        public BookingStatus BookingVanStatus { get; set; }

        //[ForeignKey("BookingStatus")]

        //public int BookingStatusId { get; set; }
        //public virtual BookingStatus BookingStatus { get; set; }

        [ForeignKey("Car")]

        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


        //public BookingStatus StatusVanBooking { get; set; }

        public BookingVehicule()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }
    }
}

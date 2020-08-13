using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using Microsoft.Data.SqlClient.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Booking
{
    public class CreateBookingViewModel : BookingVehicule // il herites les prop de cette clase mais ils sait pas qu'elle donnees il va recevoir, le controller le sait.
    {
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public int Prijs { get; set; }
        public int MinLeeftijd { get; set; }
        public int Kw { get; set; }
        public string ExistingPhotoCar { get; set; }
        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Date is mandatory")]
        //public new DateTime StartDate { get; set; }
    }
}
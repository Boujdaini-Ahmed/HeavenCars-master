using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Booking
{
    public class ReceiptEmailBodyViewModel
    {
        public string OrderNum { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ServiceName { get; set; }
        public string Hours { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}

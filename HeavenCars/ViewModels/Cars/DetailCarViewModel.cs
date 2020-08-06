using HeavenCars.DataAccessLayer.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Cars
{
    public class DetailCarViewModel
    {
        public Car Car { get; set; }
        public string PageTitle { get; set; }

       public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
    }
}

using HeavenCars.DataAccessLayer.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Cars
{
    public class DetailCarViewModel
    {
        public int CarId { get; set; }
        public int Prijs { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public string Content { get; set; }
        public string ExistingPhotoCar { get; set; }
        public string PageTitle { get; set; }
        public bool IsAvailableForRent { get; set; }

        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
    }
}

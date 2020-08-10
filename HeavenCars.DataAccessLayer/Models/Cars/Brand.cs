using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Cars
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        //[ForeignKey("Car")]
        //public int? CarId { get; set; }

        //public virtual Car Car { get; set; }

       

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Cars
{
    public class Brand
    {
        public int BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }    

    }
}
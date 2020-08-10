using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Cars
{
    public class CarModel
    {
        
        [Key]
        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public int Kw { get; set; }
        public int MinLeeftijd { get; set; }

        [ForeignKey("Brand")]
        public int? BrandId { get; set; }

        public virtual Brand Brand { get; set; }


    }
}

using HeavenCars.DataAccessLayer.Models.Cars;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Cars
{
    public class CreateCarViewModel : Car
    {
        
        //public string Name { get; set; }

        //public string MinLeeftijd { get; set; }

        //public string Prijs { get; set; }

        //public string Kw { get; set; }

        //public string Content { get; set; }

        public IFormFile Photo { get; set; }

        //public DateTime CreatedDate { get; set; }

    }
}

// validation (required)

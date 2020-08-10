using HeavenCars.DataAccessLayer.Models.Cars;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> ListAvaible { get; } = new List<SelectListItem>
        {
            new SelectListItem() { Text = "Yes", Value= true.ToString(), Selected = false },
            new SelectListItem { Text = "No", Value= false.ToString(), Selected = true  },
        };

    }

   
}

// validation (required)

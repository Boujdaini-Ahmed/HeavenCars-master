using HeavenCars.DataAccessLayer.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Cars
{
   
    public class Car
    {
        
        public int CarId { get; set; }
        //public string Name { get; set; }

        // marque et le model > dropdown de valuers deja dans la banque de données car  one to one  one to one-> Marque one to many > Modele

        // Class de Marque
        // Class de modele

        // class car one to one vers Marque
        // class car one to one vers modele
        //class marque one to many vers modele

        //public int MinLeeftijd { get; set; }

        public int Prijs { get; set; }

        public string PhotoCar { get; set; }

        //public int Kw { get; set; }

        public string Content { get; set; }

       

        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Delete { get; set; }
        public bool IsAvailableForRent { get; set; }

        public List<ApplicationUser> Utilisateurs { get; set; }

        //public virtual Brand Brand { get; set; }

        [ForeignKey("CarModel")]
        public int? CarModelId { get; set; }
        public virtual CarModel CarModel { get; set; }


    }
}

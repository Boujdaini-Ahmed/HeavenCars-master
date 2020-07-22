using HeavenCars.DataAccessLayer.Models.Cars;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Account
{
    public class ApplicationUser : IdentityUser

    {
        [ForeignKey("Car")]
        public int? CarId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string LastName { get; set; }

        public string City { get; set; }

        public bool Delete { get; set; }

        public virtual Car Car { get; set; }


    }
}
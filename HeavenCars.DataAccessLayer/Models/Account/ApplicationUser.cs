﻿using HeavenCars.DataAccessLayer.Models.Cars;
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
        public ApplicationUser()
        {
            Messages = new HashSet<Message>();
        }
        // one to many pour applicationUser || message
        public virtual ICollection<Message> Messages { get; set; }
        //public virtual ICollection<ChatUser> Chats { get; set; }

        [ForeignKey("Car")]
        public int? CarId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Maximum 50 characters")]
        public string LastName { get; set; }

        public string City { get; set; }
        public string Address { get; set; }

        public bool Delete { get; set; }

        public virtual Car Car { get; set; }


    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Home
{
    public class ContactViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 5)]
        public string Name { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}

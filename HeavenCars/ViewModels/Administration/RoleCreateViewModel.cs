using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.ViewModels.Administration
{
    public class RoleCreateViewModel : IdentityRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}

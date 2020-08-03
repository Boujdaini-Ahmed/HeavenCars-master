using HeavenCars.DataAccessLayer.Models.Account;
using System.Collections.Generic;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models
{
    public class Chat

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Msg> Msgs { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public ChatType Type { get; set; }
    }
}

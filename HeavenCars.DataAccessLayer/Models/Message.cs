using HeavenCars.DataAccessLayer.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime When { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser Verzender { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}

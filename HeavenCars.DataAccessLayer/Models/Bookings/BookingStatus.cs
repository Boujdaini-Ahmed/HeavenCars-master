using System;
using System.Collections.Generic;
using System.Text;

namespace HeavenCars.DataAccessLayer.Models.Bookings
{
    public enum BookingStatus
    {
        Pending, //TEXT VALUE => 0 <-
        Approval, /*=>1*/
        Rejected /*=> 2*/

    }
}

// ,new booking => booking.bookingstatus = 0

// approval system 
// Admin -> edit booking page -> disable tout les autre form field .// form il a un input field ( dropdown / select / radiobutton)
// on save clikc ( buttoon) -> edit booking -> mettre a jour valuer de bookingstatus.

// -> 0,1,2 dans db

// -> REFER TO ENUM -> valuer texte qui correspond a la valuer dans DB
// -> html bookingstatus = 0 -> kaka



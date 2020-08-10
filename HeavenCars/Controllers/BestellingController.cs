//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using HeavenCars.DataAccesLayer.Context;
//using HeavenCars.DataAccessLayer.Models.Account;
//using HeavenCars.DataAccessLayer.Models.Bestel;
//using HeavenCars.ViewModels.Bestel;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace HeavenCars.Controllers
//{
//    public class BestellingController : Controller
//    {
//        private readonly AppDbContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;


//        public BestellingController(AppDbContext context, UserManager<ApplicationUser> _UserManager)
//        {
//            _context = context;
//            _userManager = _UserManager;
//        }
//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpGet("/Bestel/Bestelling/{id}")]
//        public async Task<IActionResult> CheckOut(int? id)
//        {

//            //data to prepare the checkout and invioce , payment
//            //BookingVehicule _OrderBooking = await _context.BookingVehicules.FindAsync(id);
//            Bestel bestel = await _context.Bestellen.FindAsync(id);
//            //if (_OrderBooking == null)
//            //{
//            //    return RedirectToAction("Error", "Home", ErrorViewModel);//error message 
//            //}


//            var _User = await _context.Users.FirstOrDefaultAsync(f => f.Id == bestel.UserId);

//            CheckOutViewModel model = new CheckOutViewModel
//            {
//                FirstName = _User.FirstName,
//                LastName = _User.LastName,
//                Email = _User.Email,
//                Mobile = _User.PhoneNumber,
//                TotalOrder = bestel.TotalAmount,
//                OrderId = bestel.Id,
//                VehicleId = bestel.VehicleId

//            };

//            return View(model);
//        }
//    }
//}

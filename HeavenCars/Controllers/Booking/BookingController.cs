using System;
using System.Linq;
using System.Threading.Tasks;
using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Repositories;
using HeavenCars.DataAccessLayer.Repositories.Cars;
using HeavenCars.ViewModels.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

namespace HeavenCars.Controllers.Bookings
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ICarRepository _carRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingController> _logger;
        private readonly AppDbContext _context;


        public BookingController(IBookingRepository bookingRepository,
                                 IWebHostEnvironment webHostEnvironment,
                                 ICarRepository carRepository,
                                 UserManager<ApplicationUser> userManager,
                                 ILogger<BookingController> logger,
                                 AppDbContext context
                             )
        {
            _bookingRepository = bookingRepository;
            _carRepository = carRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

 
        public IActionResult Calendar()
        {

            {
                return View();
            }

        }


        public JsonResult BookingCalendar()
        {

            {
                var listBookings = _context.BookingVehicules.ToList();
                
                return new JsonResult (listBookings);
            }

        }


        [HttpGet]
        public IActionResult Details(int id)
          
        {
            
                var booking = _bookingRepository.GetBooking(id);
                //var car = _carRepository.GetCar(booking.CarId);


                //var currentuser = await _userManager.GetUserAsync(HttpContext.User);
                double Total  = 1000;
                //int Days = 0;

                //Days = (booking.EndDate - booking.StartDate).Days;

               
              
                    //if (Days == 1)
                    //{
                    //    Total = Days * booking.Car.Prijs;
                    //}
                    //else if (Days == 2)
                    //{
                    //    Total = Days * Math.Round((booking.Car.Prijs * 0.8), 0);
                    //}
                    //else 
                    //{
                    //    Total = Days * booking.Car.Prijs;
                    //}
                    if(booking != null) { 

                    DetailBookingViewModel model = new DetailBookingViewModel()
                    {
                        BookingId = booking.BookingId,
                        BrandName = booking.Car.CarModel.Brand.BrandName,
                        ModelName = booking.Car.CarModel.ModelName,
                        StartDate = booking.StartDate,
                        EndDate = booking.EndDate,
                        BookingVanStatus = booking.BookingVanStatus,
                        Prijs = booking.Car.Prijs,
                        ExistingPhotoCar = booking.Car.PhotoCar,
                        ApplicationUser = booking.ApplicationUser,
                        Days = "0",
                        Total = Total.ToString(),
                        //FirstName = currentuser.FirstName,
                        //LastName = currentuser.LastName,
                        //Email = currentuser.Email,
                        //PhoneNumber = currentuser.PhoneNumber
                        FirstName = "",
                        LastName ="",
                        Email = "",
                       PhoneNumber = ""



                        //Total = _jours.TotalPrijs.ToString()
                        //Price = _jours.PricePerHour.ToString(),


                    };

                return View(model);
            };
            return View();


        }

        [HttpGet]
        public IActionResult PayementConfirmation()
        {
            
            {
                return View();

            }
        }



        public IActionResult Charge(string stripeEmail, string stripeToken/*, DetailBookingViewModel model*/)
        {

            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            
                
                
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = 500,
                Description = "Test payement",
                Currency = "usd",
                Customer = customer.Id,


            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                return RedirectToAction("PayementConfirmation", "Booking");
            }
            else
            {

            }

            return View();
        }




        [HttpGet]
        [Authorize]

        public async Task<IActionResult> CreateAsync(int carid)
        {
            // Viewmodel =>calander avec les dates et heurres -> les dates et heures deja present dans banque de données pour voiture x => viewmodel calender et dropdown heures soit grisés.
            {
                var car = _carRepository.GetCar(carid);
                    var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                    {
                    CreateBookingViewModel createBookingViewModel = new CreateBookingViewModel()
                    {

                        BrandName = car.CarModel.Brand.BrandName,
                        ModelName = car.CarModel.ModelName,
                        Prijs = car.Prijs,
                        MinLeeftijd = car.CarModel.MinLeeftijd,
                        Kw = car.CarModel.Kw,
                        ExistingPhotoCar = car.PhotoCar
                        
                           
                        };
                        return View(createBookingViewModel);
                    }
                }       
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateAsync(CreateBookingViewModel formulaire) //  Method(class nom de l'object)
        {
            // Post de booking -> model qui contient les valuers du formuliare
            // 

            {
                if (ModelState.IsValid) // si model est valide alors 
                {
                    var LoggedIn = User.IsInRole("Admin, User"); // false / true 
                    var currentuser = await _userManager.GetUserAsync(HttpContext.User); // class ApplicationUser -> USER QUI EST logged in. 

                    //if (LoggedIn) // if statement sont des check sur valuers (pas sur properties
                    // -> Gargae 1 et garage 3 => admin de garage 1 , rajouter une voiture dans garage 1 , que il est bien admin de garage 1.
                    // -> naviger vers http/local/garage2

               
                    BookingVehicule nouveaubookingquidoitallerdansbanquededonnees = new BookingVehicule
                    {
                       
                        ApplicationUserId = currentuser.Id, // Class -> template (only properties) Class avec values -> object de la classe -> currentuser est un object de la classe applicationuser.
                        CarId = formulaire.CarId,
                        StartDate = formulaire.StartDate,
                        EndDate = formulaire.EndDate,
                        CreateBy = currentuser.Email,

                    };



                    var response = _bookingRepository.Create(nouveaubookingquidoitallerdansbanquededonnees);

                    if (response != null && response.BookingId != 0)
                    {
                        return RedirectToAction("details", "Booking", new { id = nouveaubookingquidoitallerdansbanquededonnees.BookingId });
                    }

                    return View("NotAuthorized");
                }
                return View(formulaire);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int bookingid)
        {
            try
            {

                var booking = _bookingRepository.GetBooking(bookingid);

                var currentuser = await _userManager.GetUserAsync(HttpContext.User);



                var editBookingViewModel = new EditBookingViewModel
                {
                    Car = booking.Car,
                    BookingId = booking.BookingId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    BookingVanStatus = booking.BookingVanStatus



                };

                return View(editBookingViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving to edit booking.");
                throw;

            }

        }
        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookingViewModel editModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    BookingVehicule booking = _bookingRepository.GetBooking(editModel.BookingId);
                    var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                    booking.Car = editModel.Car;
                    booking.StartDate = editModel.StartDate;
                    booking.EndDate = editModel.EndDate;
                    booking.BookingVanStatus = editModel.BookingVanStatus;



                    _bookingRepository.Update(booking);


                    return RedirectToAction("BookingsList", "Booking");



                }
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving to edit booking.");
                throw;

            }
        }


        [Authorize(Roles = "Admin")]
        public IActionResult BookingsList()
        {
            try
            {
               
                var booking = _bookingRepository.GetAllBookings();
                return View(booking);
                
            }
    
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving booking list admin.");
                throw;

            }
        }


        public IActionResult HistoryBooking()
        {
            try
            {
                var currentuser = _userManager.GetUserId(HttpContext.User);
                var booking = _context.BookingVehicules.Where(user => user.ApplicationUserId == currentuser);
                var car = _carRepository.GetAllCars();
                return View(booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving booking list user.");
                throw;

            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int Bookingid)
        {
            try
            {
                BookingVehicule bookingVehicule = _bookingRepository.GetBooking(Bookingid);

                return View(bookingVehicule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When getting the delete page of a booking.");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteShure(int BookingId)
        {
            try
            {
            
                var booking = _bookingRepository.GetBooking(BookingId);
                var response = _bookingRepository.Delete(booking);

                if (response != null && response.CarId != 0)
                {
                    return RedirectToAction("BookingsList");
                }

                return View("Delete", booking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When deleting a booking.");
                throw;
            }
        }
    }
    }

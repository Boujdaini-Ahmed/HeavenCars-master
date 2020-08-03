using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Repositories;
using HeavenCars.DataAccessLayer.Repositories.Cars;
using HeavenCars.ViewModels.Booking;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
                                 AppDbContext context)
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

        public IActionResult BookingsList(string search = null)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var foundCars = _carRepository.SearchCars(search);
                return View(foundCars);
            }
            var booking = _bookingRepository.GetAllBookings();
            return View(booking);
        }

        //[HttpGet]
        //public async Task<IActionResult> BookingsListAsync()
        //{

        //    var currentuser = await _userManager.GetUserAsync(HttpContext.User); Cookiepolicy (UseriD username) -> MSDN official cookie setting. 

        //    try
        //    {
        //        if (User.IsInRole("SuperAdmin"))
        //        {
        //            var bookingList = _bookingRepository.GetAllBookings();
        //            return View(bookingList);
        //        }
        //        else if (User.IsInRole("Admin"))
        //        {
        //            var bookingList = _bookingRepository.GetAllBookings().Where(x => x.CarId == currentuser.CarId);

        //            return View(bookingList);
        //        }
        //        else
        //        {
        //            var bookingList = _bookingRepository.GetAllBookings();
        //            ////.Where(x => x.BoardUsers.Any(x => x.ApplicationUserId == currentuser.Id))
        //            ////.ToList();

        //            return View(bookingList);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _logger.LogError(ex, $"When getting the list of boards.");
        //        throw;
        //    }

        //}

  
        public async Task<IActionResult> Details(int id)

        {

            {
                var booking = _bookingRepository.GetBooking(id);
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);
                //var UserBoards = _boardRepository.GetAllBoardsfromcompany(currentuser.CompanyId).Where(x => x.BoardUsers.Any(x => x.ApplicationUserId == currentuser.Id && x.IsDeleted == false)).ToList();

                //if (booking == null )
                //{
                //    Response.StatusCode = 404;
                //    return View("BookingNotFound", id);
                //}
                //else if (User.IsInRole("SuperAdmin") ||
                //    User.IsInRole("Admin") && booking.CarId == currentuser.CarId
                //    || User.IsInRole("User") && booking.CarId == currentuser.CarId
                //    )
                {

                    DetailBookingViewModel detailBookingViewModel = new DetailBookingViewModel()
                    {
                        BookingVehicule = booking


                    };
                    return View(detailBookingViewModel);
                }
                ////return View("NotAuthorized");

            }
        }




        [HttpGet]

        public async Task<IActionResult> CreateAsync(int carid)
        {
            
                {
                    var car = _carRepository.GetCar(carid);
                    var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                    // var dispnibilité : repository.get.booking(voiture x). -> viewmodel
                    // Viewmodel =>calander avec les dates et heurres -> les dates et heures deja present dans banque de données pour voiture x => viewmodel calender et dropdown heures soit grisés.

                    //if (car == null)
                    //{
                    //    Response.StatusCode = 404;
                    //    return View("../Cars/CarNotFound", carid);
                    //}
                    //else if (User.IsInRole("SuperAdmin") ||
                    //   User.IsInRole("Admin") && carid == currentuser.CarId)


                  

                    {
                        CreateBookingViewModel createBookingViewModel = new CreateBookingViewModel()
                        {

                            Car = car,

                        };
                        return View(createBookingViewModel);
                    }
                    //return View("NotAuthorized");

                }
            
        }

        [HttpPost]
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
                        return RedirectToAction("details", new { id = nouveaubookingquidoitallerdansbanquededonnees.BookingId });
                    }

                    return View("NotAuthorized");
                }
                return View(formulaire);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            
                var booking = _bookingRepository.GetBooking(id);
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);



                var editBookingViewModel = new EditBookingViewModel
                {
                    BookingId = booking.BookingId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    BookingVanStatus = booking.BookingVanStatus



                };

                return View(editBookingViewModel);

            }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBookingViewModel editModel)
        {
            

                    if (ModelState.IsValid)
                    {
                        BookingVehicule booking = _bookingRepository.GetBooking(editModel.BookingId);
                        var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                        booking.StartDate = editModel.StartDate;
                        booking.EndDate = editModel.EndDate;
                        booking.BookingVanStatus = editModel.BookingVanStatus;



                         _bookingRepository.Update(booking);

                       
                            return RedirectToAction("BookingsList", new { id = booking.BookingId });
                        
                      
                    
                }
                return View();
            }
        
        }
    }

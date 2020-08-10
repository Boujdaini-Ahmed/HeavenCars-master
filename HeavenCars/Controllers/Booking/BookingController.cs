using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models;
using HeavenCars.DataAccessLayer.Models.Account;
using HeavenCars.DataAccessLayer.Models.Bookings;
using HeavenCars.DataAccessLayer.Models.Cars;
using HeavenCars.DataAccessLayer.Repositories;
using HeavenCars.DataAccessLayer.Repositories.Cars;
using HeavenCars.Services;
using HeavenCars.ViewModels.Booking;
using HeavenCars.ViewModels.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;


        public BookingController(IBookingRepository bookingRepository,
                                 IWebHostEnvironment webHostEnvironment,
                                 ICarRepository carRepository,
                                 UserManager<ApplicationUser> userManager,
                                 ILogger<BookingController> logger,
                                 AppDbContext context,
                                 IEmailSender emailSender)
        {
            _bookingRepository = bookingRepository;
            _carRepository = carRepository;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
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



        [HttpGet("/Booking/Details/{id}")]
        public async Task<IActionResult> Details(int id)

        {


            {
                
                BookingVehicule _jours = _context.BookingVehicules.Find(id);
                Car Car = _context.Cars.Find(_jours.CarId);
                var car = _carRepository.GetCar(_jours.CarId);
                BookingVehicule bestelling = await _context.BookingVehicules.FindAsync(id);
               

                var booking = _bookingRepository.GetBooking(id);
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);
                double Total ;
                int Days = (_jours.EndDate - _jours.StartDate).Days;

               
                {
                    if (Days == 1)
                    {
                        Total = Days * car.Prijs;
                    }
                    else if (Days == 2)
                    {
                        Total = Days * Math.Round((car.Prijs * 0.8), 0);
                    };

                    var _User = await _context.Users.FirstOrDefaultAsync();

                    DetailBookingViewModel detailBookingViewModel = new DetailBookingViewModel()
                    {
                        BookingVehicule = booking,
                        Days = Days.ToString(),
                        Price = Car.Prijs.ToString(),
                        Total = Total.ToString(),
                        FirstName = _User.FirstName,
                        LastName = _User.LastName,
                        Email = _User.Email,
                        Mobile = _User.PhoneNumber,
                        TotalOrder = bestelling.TotalAmount,
                        OrderId = bestelling.BookingId,
                        VehicleId = bestelling.CarId

                        //Total = _jours.TotalPrijs.ToString()
                        //Price = _jours.PricePerHour.ToString(),


                    };
                    return View(detailBookingViewModel);
                }
                ////return View("NotAuthorized");

            }
        }

        public IActionResult PaymentConfirmation(int id)
        {
            ErrorViewModel errorViewModel = new ErrorViewModel();
            if (id.Equals(0))
            {
                errorViewModel.ErrorMessage = "Something went wrong while confirming your order.Contact Support";
                return RedirectToAction("ErrorPagina", "Home", errorViewModel);//error message 
            }
            BookingVehicule bestelling = new BookingVehicule();
            bestelling = _context.BookingVehicules.Find(id);
            return View(bestelling);
        }



        public async Task<IActionResult> PaymentCard(string stripeEmail, string stripeToken, DetailBookingViewModel model)
        {
            
            var currentuser =  _userManager.GetUserId(HttpContext.User);
            int? _amount = 0;
            if (!string.IsNullOrEmpty(model.TotalOrder.ToString()))
            {
                double n;
                bool isNumeric = double.TryParse(model.TotalOrder.ToString(), out n);
                _amount = isNumeric ? (int)(Convert.ToDecimal(model.TotalOrder.ToString()) * 100) : -1;
            }
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            
                
                
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = _amount,
                Description = model.Comment,
                Currency = "usd",
                Customer = customer.Id,
                ReceiptEmail = model.Email,
                Metadata = new Dictionary<String, String>()
                {
                   { "OrderId", model.OrderId.ToString()},
                           
                },


            });

            if (charge.Status == "succeeded")
            {
                string BalanceTransactionId = charge.BalanceTransactionId;
                BookingVehicule bestelling = await _context.BookingVehicules.FindAsync(model.BookingId);

                Transaction _Transaction = new Transaction
                {
                    TransactionId = BalanceTransactionId,
                    UserId = currentuser,
                    OrderId = model.OrderId,
                    Amount = model.TotalOrder,
                    Status = "succeeded",
                    Date = DateTime.Now,
                    FullName = model.FirstName + " " + model.LastName
                };

                await _context.AddAsync(_Transaction);
                await _context.SaveChangesAsync();

                Car car = await _context.Cars.FindAsync(model.CarId);


                ReceiptEmailBodyViewModel EmailReceipt = new ReceiptEmailBodyViewModel()
                {
                    OrderNum = model.OrderId.ToString(),
                    CustomerName = model.FirstName + " " + model.LastName,
                    CustomerEmail = model.Email,
                    Price = car.Prijs.ToString(),
                    Total = model.TotalOrder.ToString(),
                   
                };

                string EmailReciptsBody = EmailReceiptsBody(EmailReceipt);
                await _emailSender.SendEmailAsync(model.Email, "Car Share Service", EmailReciptsBody);

               
                //return RedirectToAction("PayementConfirmation", "Booking", new { id = model.BookingId }); 
            }
            else
            {

            }

            return View();
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
                    Car = booking.Car,
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

        private string EmailReceiptsBody(ReceiptEmailBodyViewModel _EmailReceipts)
        {
            var webRoot = _webHostEnvironment.WebRootPath;

            string body = string.Empty;
            string TemplePath = webRoot + "/Pages/EmailReceipts.html";
            using (StreamReader reader = new StreamReader(TemplePath))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{InvoiceNumber}", _EmailReceipts.OrderNum);
            body = body.Replace("{Date}", DateTime.Now.ToShortDateString());
            body = body.Replace("{CustomerName}", _EmailReceipts.CustomerName);
            body = body.Replace("{CustomerEmail}", _EmailReceipts.CustomerEmail);
            body = body.Replace("{ServiceName}", _EmailReceipts.ServiceName);
            body = body.Replace("{Hours}", _EmailReceipts.Hours);
            body = body.Replace("{Price}", _EmailReceipts.Price);
            body = body.Replace("{Total}", _EmailReceipts.Total);
   
     

            return body;
        }
    }
    }

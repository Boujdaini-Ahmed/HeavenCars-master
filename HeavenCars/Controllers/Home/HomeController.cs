using System;
using System.Threading.Tasks;
using HeavenCars.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using HeavenCars.ViewModels.Home;
using MimeKit.Text;
using HeavenCars.DataAccessLayer.Models.Account;
using Microsoft.AspNetCore.Identity;
using HeavenCars.DataAccesLayer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace HeavenCars.Controllers.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

   
        public async Task<IActionResult> Chat()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUserName = currentUser.UserName;
            }
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> Create(Message message)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                message.UserName = User.Identity.Name;
                var zender = await _userManager.GetUserAsync(User);
                message.UserID = zender.Id;
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Error();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When trying to create a new message.");
                throw;
            }

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public async Task <IActionResult> Contact(string id)
        {

                var user = await _userManager.FindByIdAsync(id);
                var model = new ContactViewModel
                {


                    Email = user.Email


                };

                return View(model);
            }


        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.Id);


                    user.Email = model.Email;




                    var message = new MimeMessage(); //instancie un nouveau mimeMessage

                    message.To.Add(new MailboxAddress("E-mail Recipient Name", "ict.slap@gmail.com")); //va regler le TO email addresse dans le barre

                    message.From.Add(new MailboxAddress("E-mail From Name", user.Email));   //va regler le FROM email addresse dans la barre

                    message.Subject = model.Subject; //C'est le subject de ton email

                    message.Body = new TextPart(TextFormat.Html)  //C'est le body message de ton email
                    {
                        Text = model.Message
                        + " Message was sent by: " + model.Name + " E-mail: " + model.Email
                    };

                    //Va configurer le email
                    using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        emailClient.Connect("smtp.gmail.com", 587, false);
                        emailClient.Authenticate("ict.slap@gmail.com", "Zntdu1790");
                        emailClient.Send(message);
                        emailClient.Disconnect(true);
                    }


                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving to contact the admin.");
                throw;

            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                    return View("NotFound");
                }




                var model = new PersoInformationViewModel
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address

                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving to edit your profile.");
                throw;

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersoInformationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByIdAsync(model.Id);

                    if (user == null)
                    {
                        ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                        return View("NotFound");
                    }

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Address = model.Address;



                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving to edit your profile.");
                throw;

            }
        }

        
        }

    }



    
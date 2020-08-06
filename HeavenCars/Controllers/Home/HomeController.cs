using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HeavenCars.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;
using HeavenCars.ViewModels.Home;
using MimeKit.Text;
using HeavenCars.DataAccessLayer.Models.Account;
using Microsoft.AspNetCore.Identity;
using HeavenCars.DataAccesLayer.Context;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers.Wrappers;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Security.Claims;
using HeavenCars.DataAccessLayer.Repositories;
using MimeKit.Encodings;

namespace HeavenCars.Controllers.Home
{
    [Authorize]
    public class HomeController : Controller
    {
        public readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IChatRepository _chatRepository;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager/*, IChatRepository chatRepository*/)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            //_chatRepository = chatRepository;
        }
        public IActionResult Index()
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel contactViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var message = new MimeMessage(); //instancie un nouveau mimeMessage

                    message.To.Add(new MailboxAddress("E-mail Recipient Name", "ict.slap@gmail.com")); //va regler le TO email addresse dans le barre

                    message.From.Add(new MailboxAddress("E-mail From Name", "from@domain.com"));   //va regler le FROM email addresse dans la barre

                    message.Subject = contactViewModel.Subject; //C'est le subject de ton email

                    message.Body = new TextPart(TextFormat.Html)  //C'est le body message de ton email
                    {
                        Text = contactViewModel.Message + " Message was sent by: " + contactViewModel.Name + " E-mail: " + contactViewModel.Email
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
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Oops! We have a problem here {ex.Message}";
                }

            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
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

        [HttpPost]
        public async Task<IActionResult> Edit(PersoInformationViewModel model)
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

        //public IActionResult Intro()
        //{
        //    var users = _userManager.Users.ToList();
        //    return View(users);
        //}


        //[HttpPost]
        //public async Task<IActionResult> CreateRoom(string name)
        //{


        //    _context.Chats.Add(new Chat
        //    {
        //        Name = name,
        //        Type = ChatType.Private
        //    });

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Intro");
        //}

        //[HttpGet]

        //public IActionResult Chatter(int id)
        //{
        //    var chat = _context.Chats
        //        .Include(x => x.Msgs)
        //        .FirstOrDefault(x => x.Id == id);
            
        //    return View(chat);
        //}

        //[HttpPost]

        //public async Task<IActionResult> CreateMessage(int chatId, string message)
        //{
        //    var Message = new Msg
        //    {
        //        ChatId = chatId,
        //        Text = message,
        //        Name = "Default",
        //        Timestamp = DateTime.Now
        //    };

        //    _context.Msgs.Add(Message);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("Chatter", new { id = chatId });
        //}



        //public IActionResult RoomsDefault()
        //{

        //    var chats = _chatRepository.GetAllChat();
        //    return View(chats);
        //}
        //public IActionResult Find()
        //{
        //    var users = _context.Users
        //        .Where(x => x.Id != User.FindFirst(ClaimTypes.NameIdentifier).Value)
        //        .ToList();

        //    return View(users);
        //}

        //public IActionResult Private()
        //{
        //    var chats = _context.Chats
        //        .Include(x => x.ApplicationUsers)
        //        .ThenInclude(x => x.User)
        //        .ToList();

        //    return View(chats);
        }
        //public IActionResult CreatePrivateRoom(string userId)
        //{
        //    var chat = new Chat
        //    {
        //        Type = ChatType.Private
        //    };

        //    chat.ApplicationUsers.Add(new ChatUser
        //    {
        //        UserId = userId
        //    });

        //    return View(users);
        //}
        //    [HttpGet]
        //    public async Task<IActionResult> Edit()
        //    {
        //        var user = await _userManager.GetUserAsync(User);
        //        if (user == null)
        //        {
        //            throw new ApplicationException($"Unable to load user with ID ' { _userManager.GetUserId(User)} '.");
        //        }
        //        var model = new PersoInformationViewModel
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //            EmailConfirmed = user.EmailConfirmed,
        //            PhoneNumber = user.PhoneNumber,
        //            Address = user.Address

        //        };

        //        return View(model);
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(PersoInformationViewModel model)
        //    {
        //    //    if (ModelState.IsValid)
        //    //    {
        //    //        return View(model);
        //    //    }

        //    //    var user = await _userManager.GetUserAsync(User);
        //    //    user.Address = model.Address;
        //    //    if (user == null)
        //    //    {
        //    //        throw new ApplicationException($"Unable to load user with ID ' { _userManager.GetUserId(User)} '.");
        //    //    }

        //    //    var email = model.Email;
        //    //    if (model.Email != email)
        //    //    {
        //    //        var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
        //    //        if (!setEmailResult.Succeeded)
        //    //        {
        //    //            throw new ApplicationException($"Unexpected error occured setting email' {user.Id} '.");
        //    //        }

        //    //    }

        //    //    var phoneNumber = model.PhoneNumber;
        //    //    if (model.PhoneNumber != phoneNumber)
        //    //    {
        //    //        var setPhoneNumberResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
        //    //        if (!setPhoneNumberResult.Succeeded)
        //    //        {
        //    //            throw new ApplicationException($"Unexpected error occured setting phone number with user with ID' {user.Id} '.");
        //    //        }

        //    //    }
        //    //    await _userManager.UpdateAsync(user);

        //    //    return RedirectToAction("Edit", "Home");
        //    //}

        //    //    if (user == null)
        //    //    {
        //    //        ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
        //    //        return View("NotFound");
        //    //    }
        //    //    user.FirstName = model.FirstName;
        //    //    user.LastName = model.LastName;
        //    //    user.Email = model.Email;
        //    //    user.EmailConfirmed = model.EmailConfirmed;
        //    //    user.UserName = model.UserName;
        //    //    user.City = model.City;

        //    //    var result = await _userManager.UpdateAsync(user);

        //    //    if (result.Succeeded)
        //    //    {
        //    //        return RedirectToAction("Index", "Home");
        //    //    }

        //    //    foreach (var error in result.Errors)
        //    //    {
        //    //        ModelState.AddModelError(string.Empty, error.Description);
        //    //    }
        //    //}


        //}
    }



    
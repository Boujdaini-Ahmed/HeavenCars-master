using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Cars;
using HeavenCars.DataAccessLayer.Repositories.Cars;
using HeavenCars.ViewModels.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HeavenCars.Controllers.Cars
{

    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<CarsController> _logger;
        private readonly AppDbContext _context;

        public CarsController(ICarRepository carRepository,
            IWebHostEnvironment webHostEnvironment, AppDbContext context, ILogger<CarsController> logger)
        {
            _carRepository = carRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _logger = logger;
        }

        public IActionResult ListCars(string sortOrder, string search = null)
        {
            try
            {
                // this is an action method which will help you retrive data from EntityFramework and two viewdata storage variable is used to manage sorting on the page. 
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["PrijsSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prijs_desc" : "";
                var list = _carRepository.GetAllCars();

                switch (sortOrder)
                {
                    case "name_desc":
                        list = list.OrderByDescending(x => x.CarModel.Brand.BrandName);
                        break;
                    case "prijs_desc":
                        list = list.OrderByDescending(x => x.Prijs);
                        break;
                    default:
                        list = list.OrderBy(x => x.CarModel.Brand.BrandName);
                        break;
                }

                if (!string.IsNullOrEmpty(search))
                {
                    var foundCars = _carRepository.SearchCars(search);
                    return View(foundCars);
                }

                return View(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving Cars List.");
                throw;
            }
        
            
            }

        public IActionResult ListCarsAdmin()
        {
            try { 
            IEnumerable<Car> car = _carRepository.GetAllCars();
            return View(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When retrieving Car List Admin.");
                throw;

            }
        }

        public ViewResult Details(int id)

        {
            try
            {
                var car = _carRepository.GetCar(id);

                if (car == null)
                {
                    Response.StatusCode = 404;
                    return View("CarNotFound", id);
                }
              
                DetailCarViewModel detailCarViewModel = new DetailCarViewModel()
                {
                    CarId = car.CarId,
                    BrandName = car.CarModel.Brand.BrandName,
                    ModelName = car.CarModel.ModelName,
                    Prijs = car.Prijs,
                    Content = car.Content,
                    ExistingPhotoCar = car.PhotoCar,
                    IsAvailableForRent = car.IsAvailableForRent,
                    PageTitle = "Car Detail",
                    Price1 = Math.Round((car.Prijs * 0.9), 0),
                    Price2 = Math.Round((car.Prijs * 0.8), 0),
                    Price3 = Math.Round((car.Prijs * 0.7), 0),
                };
                return View(detailCarViewModel);
          
            }
       
            
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When trying to details of a car.");
                throw;
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateCarViewModel model)
        {
            try 
            { 
                if (ModelState.IsValid)
                 {
                string uniqueFileName = ProcessUploadFile(model);

                Car newCar = new Car
                {
        
                   CarModel = model.CarModel,
                    Prijs = model.Prijs,
                    Content = model.Content,
                    PhotoCar = uniqueFileName,
                    CreatedDate = DateTime.Now,
                    IsAvailableForRent = true,

                };

                _carRepository.Add(newCar);
                return RedirectToAction("Details", "Cars", new { id = newCar.CarId });
            }
            return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When trying to create a new car.");
                throw;
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ViewResult Edit(int Carid) /*Return Type attend comme reponse */
        {
            try
            { 
            var car = _carRepository.GetCar(Carid);
            var editCarViewModel = new EditCarViewModel
            {
                CarModel = car.CarModel,
                CarId = car.CarId,
                IsAvailableForRent = car.IsAvailableForRent,
                Prijs = car.Prijs,
                Content = car.Content,
                ExistingPhotoCar = car.PhotoCar,
                UpdateDate = DateTime.Now

            };
          
            return View(editCarViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When getting the update  of a car.");
                throw;
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditCarViewModel model)
        {
            try
            { 
            if (ModelState.IsValid)
            {
                Car car = _carRepository.GetCar(model.CarId);


                car.IsAvailableForRent = model.IsAvailableForRent;
                car.Prijs = model.Prijs;
                car.Content = model.Content;
                car.UpdateDate = DateTime.Now;


                if (model.Photo != null)
                {
                    if (model.ExistingPhotoCar != null)

                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            "images", model.ExistingPhotoCar);
                        System.IO.File.Delete(filePath);
                    }

                    car.PhotoCar = ProcessUploadFile(model);

                }

                _carRepository.Update(car);
                return RedirectToAction("ListCars", "Cars");
            }
            return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When trying to update a car.");
                throw;
            }

        }

        // public ViewResult Edit(int id)
        // private string Processupload(datatype du parameter nom du parameter)
        private string ProcessUploadFile(CreateCarViewModel model) // return type
        {
            string uniqueFileName = null; // creer var comme type string dont la valeur et nul.
            if (model.Photo != null) // objet model avec property photo/ je check si la valeur de ma property photo qui se trouve dans l'objet model existe.

            /*
             object objectname :
            Prop = value
            Id = 1
            Name = 'test'

            if(objectname.Name (==)== 'custom')
             */
            {
                string pathnameofuploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(pathnameofuploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }

            }

            return uniqueFileName;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(int Carid)
        {
            try
            { 
            Car car = _carRepository.GetCar(Carid);

            return View(car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When getting the delete page of a car.");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteShure(int CarId)
        {
            try
            { 
    
            var car = _carRepository.GetCar(CarId);
            var response = _carRepository.Delete(car);



            if (response != null && response.CarId != 0)
            {
                return RedirectToAction("ListCarsAdmin");
            }

            return View("Delete", car);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"When deleting a car.");
                throw;
            }
        }
    }
}


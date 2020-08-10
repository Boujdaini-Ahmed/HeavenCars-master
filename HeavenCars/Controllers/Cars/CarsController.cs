using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Cars;
using HeavenCars.DataAccessLayer.Repositories.Cars;
using HeavenCars.ViewModels.Cars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;


namespace HeavenCars.Controllers.Cars
{

    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public CarsController(ICarRepository carRepository,
            IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _carRepository = carRepository;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        public IActionResult ListCars(string sortOrder, string search = null)
        {
            // this is an action method which will help you retrive data from EntityFramework and two viewdata storage variable is used to manage sorting on the page. 
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PrijsSortParm"] = String.IsNullOrEmpty(sortOrder) ? "prijs_desc" : "";
            var list = from s in _context.Cars
                       select s;

            switch (sortOrder)
            {
                case "name_desc":
                    list = list.OrderByDescending(s => s.CarModel.Brand.BrandName);
                    break;
                case "prijs_desc":
                    list = list.OrderByDescending(s => s.Prijs);
                    break;
                default:
                    list = list.OrderBy(s => s.CarModel.Brand.BrandName);
                    break;


            }



            if (!string.IsNullOrEmpty(search))
            {
                var foundCars = _carRepository.SearchCars(search);
                return View(foundCars);
            }
            IEnumerable<Car> car = _carRepository.GetAllCars();
            return View(car);
        }

        public IActionResult ListCarsAdmin()
        {
            IEnumerable<Car> car = _carRepository.GetAllCars();
            return View(car);
        }

        public ViewResult Details(int Id)

        {
            var car = _carRepository.GetCar(Id);

            if (car == null)
            {
                Response.StatusCode = 404;
                return View("CarNotFound", Id);
            }

            DetailCarViewModel detailCarViewModel = new DetailCarViewModel()
            {
                Car = car,
                PageTitle = "Car Detail",
                Price1 = Math.Round((car.Prijs * 0.9), 0),
                Price2 = Math.Round((car.Prijs * 0.8), 0),
                Price3 = Math.Round((car.Prijs * 0.7), 0),
            };
            return View(detailCarViewModel);
        }

        [Authorize]
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateCarViewModel model)
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

                };

                _carRepository.Add(newCar);
                return RedirectToAction("Details", "Cars", new { id = newCar.CarId });
            }
            return View();
        }

        [Authorize]
        [HttpGet]
        public ViewResult Edit(int Carid) /*Return Type attend comme reponse */
        {
            var car = _carRepository.GetCar(Carid);
            var editCarViewModel = new EditCarViewModel
            {
                CarModel = car.CarModel,
                CarId = car.CarId,
                IsAvailableForRent = car.IsAvailableForRent,
                Prijs = car.Prijs,
                ExistingPhotoCar = car.PhotoCar,
                UpdateDate = DateTime.Now


            };
          
            return View(editCarViewModel);
        }

        [Authorize]
        [HttpPost]

        public IActionResult Edit(EditCarViewModel model)
        {
            if (ModelState.IsValid)
            {
                Car car = _carRepository.GetCar(model.CarId);

                //CarModel = car.CarModel,
                //car.IsAvailableForRent = model.IsAvailableForRent;
                car.Prijs = model.Prijs;
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

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Car car = _carRepository.GetCar(Id);

            return View(car);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteShure(int Id)
        {
            var car = _carRepository.GetCar(Id);

            car.Delete = true;

            var response = _carRepository.Delete(car);



            if (response != null && response.CarId != 0)
            {
                return RedirectToAction("ListCars");
            }

            return View("Delete", car);
        }
    }
}


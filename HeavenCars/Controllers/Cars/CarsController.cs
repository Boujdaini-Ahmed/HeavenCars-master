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
                    list = list.OrderByDescending(s => s.Name);
                    break;
                case "prijs_desc":
                    list = list.OrderByDescending(s => s.Prijs);
                    break;
                default:
                    list = list.OrderBy(s => s.Name);
                    break;

                    
            }

            

            if (!string.IsNullOrEmpty(search))
            {
                var foundCars = _carRepository.SearchCars(search);
                return View(foundCars);
            }
            var cars = _carRepository.GetAllCars();
            return View(list);
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
                PageTitle = "Car Detail"
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

                    Name = model.Name,
                    MinLeeftijd = model.MinLeeftijd,
                    Prijs = model.Prijs,
                    Kw = model.Kw,
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
            var car = _carRepository.GetCarById(Carid);
            var editCarViewModel = new EditCarViewModel
            {
                CarId = car.CarId,
                Name = car.Name,
                MinLeeftijd = car.MinLeeftijd,
                Prijs = car.Prijs,
                Kw = car.Kw,
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
                Car car = _carRepository.GetCarById(model.CarId);
                car.Name = model.Name;
                car.MinLeeftijd = model.MinLeeftijd;
                car.Prijs = model.Prijs;
                car.Kw = model.Kw;
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


using HeavenCars.DataAccesLayer.Context;
using HeavenCars.DataAccessLayer.Models.Cars;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeavenCars.DataAccessLayer.Repositories.Cars
{
    public class CarRepository : ICarRepository
    {

        private readonly AppDbContext context;

        public CarRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Car Add(Car car)
        {
            context.Cars.Add(car);
            context.SaveChanges();
            return car;
        }

        public Car Delete(Car car)
        {
            Car carToBeDeleted = context.Cars.Find(car.CarId);
            if (carToBeDeleted != null)
            {
                context.Cars.Update(carToBeDeleted);
                context.SaveChanges();
            }
            return carToBeDeleted;
        }


        public IEnumerable<Car> GetAllCars()
        {
            var car = context.Cars
                .Include(x => x.CarModel).ThenInclude(x => x.Brand)
                
                .ToList();


            return car;
        }


        public Car GetCar(int Id)
        {
            var car = context.Cars
                .Include(x => x.CarModel).ThenInclude(x => x.Brand)
                .Where(x => x.CarId == Id).FirstOrDefault();
                
            return car;
        }



        public List<Car> SearchCars(string search)
        {
            

            return context.Cars
                .Include(x => x.CarModel).ThenInclude(x => x.Brand)
                .Where(p => p.CarModel.Brand.BrandName.Contains(search))
                

                .ToList();
                
        }

        public Car Update(Car carNew)
        {
            var car = context.Cars.Attach(carNew);
            car.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return carNew;
        }
        // First push to github
        // ouvrir HeavenCars en bas a droite
        // pousser sur maison 
        // pousser sur change
        // message
        // Commit and PUSH ALL

        // test Commit all
    }
}
    


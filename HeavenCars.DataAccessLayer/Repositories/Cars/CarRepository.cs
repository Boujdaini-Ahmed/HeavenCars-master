﻿using HeavenCars.DataAccesLayer.Context;
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
            return context.Cars;
        }


        public Car GetCar(int Id)
        {
            var car = context.Cars.Find(Id);
            return car;
        }

        public Car GetCarById(int id)
        {
            var car = context.Cars.Find(id);
            return car;
        }

        public List<Car> SearchCars(string search)
        {
            

            return context.Cars
                .Where(p => p.Name.Contains(search))
                

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
    


//using HeavenCars.DataAccesLayer.Context;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace HeavenCars.ViewComponents
//{
//    public class RoomViewComponent : ViewComponent
//    {
//        private AppDbContext _context;

//        public RoomViewComponent(AppDbContext context)
//        {
//            _context = context;
//        }

//        public IViewComponentResult Inovke()
//        {
//            var chats = _context.Chats.ToList();
//            return View(chats);
//        }
//    }
//}

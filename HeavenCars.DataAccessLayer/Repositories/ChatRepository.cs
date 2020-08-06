//using HeavenCars.DataAccesLayer.Context;
//using HeavenCars.DataAccessLayer.Models;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace HeavenCars.DataAccessLayer.Repositories
//{
//   public class ChatRepository : IChatRepository
//    {
//        private readonly AppDbContext context;
//        private readonly ILogger<ChatRepository> _logger;

//        public ChatRepository(AppDbContext context, ILogger<ChatRepository> logger)
//        {
//            this.context = context;
//            _logger = logger;
//        }

//        public IEnumerable<Chat> GetAllChat()
//        {
//            return context.Chats;
//        }

//        public IEnumerable<ChatUser> GetUsersofChat(int id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

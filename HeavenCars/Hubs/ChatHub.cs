using HeavenCars.DataAccessLayer.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeavenCars.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(Message message) =>

           await Clients.All.SendAsync("ReceiveMessage", message);
    }

    //public Task SendPrivateMessage(string user, string message)
    //{
    //    return Clients.User(user).SendAsync("ReceiveMessage", message);

    //}

}


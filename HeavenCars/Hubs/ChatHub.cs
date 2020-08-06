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


    //        public async Task SendMessage(string userId)
    //    {
    //        var message = $"send message to you with user id {userId }";
    //        await Clients.Client(userId).SendAsync("ReceiveMessage", message);
    //}

    //    public string GetConnectionId()
    //    {
    //        return Context.ConnectionId;
    //    }

}





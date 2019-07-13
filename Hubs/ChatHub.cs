using Employees.Data;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Hubs
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// The SignalR hub 
    /// </summary>
    public class ChatHub : Hub
    {
        /// <summary>
        /// The Hub URL for chat client
        /// </summary>
        public const string HUBURL = "/chathub";

        protected GlobalAppState GlobalAppState { get; set; }

        /// <summary>
        /// connectionId-to-username lookup
        /// </summary>
        /// <remarks>
        /// Needs to be static as the chat is created dynamically a lot
        /// </remarks>
        private static readonly Dictionary<string, User> userLookup = new Dictionary<string, User>();

        /// <summary>
        /// Send a message to all clients
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string username, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }

        /// <summary>
        /// Register username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task Register(int userId, string userName)
        {
            var currentId = Context.ConnectionId;
            if (!userLookup.ContainsKey(currentId))
            {
                // maintain a lookup of connectionId-to-username
                userLookup.Add(currentId, new User { Id = userId, Name = userName });
                // re-use existing message for now
                await Clients.AllExcept(currentId).SendAsync("ReceiveMessage", userId, userName, $"{userName} joined the chat");
            }
        }

        /// <summary>
        /// Log connection
        /// </summary>
        /// <returns></returns>
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// Log disconnection
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception e)
        {
            Console.WriteLine($"Disconnected {e?.Message}");
            // try to get connection
            string id = Context.ConnectionId;
            if (userLookup.TryGetValue(id, out User user))
            {
                GlobalAppState.Employees
                    .Find(ab => ab.F0601161_AN8 == user.Id)
                    .Connected = false;
                userLookup.Remove(id);
                await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMessage", user.Id, user.Name, $"{user.Name} has left the chat");
            }
            await base.OnDisconnectedAsync(e);
        }

        public ChatHub(GlobalAppState globalAppState)
        {
            GlobalAppState = globalAppState;
        }
    }
}

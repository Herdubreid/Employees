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
        public List<string> Groups { get; set; }
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

        protected static long MessageId { get; set; } = 0;

        protected string[] GetGroups(int userId)
        {
            var employee = GlobalAppState.Employees
                .Find(e => e.F0601161_AN8 == userId);
            var team = GlobalAppState.Employees
                .Where(e => e.F0601161_ANPA == userId)
                .Count();
            if (team > 0)
            {
                return new string[] { employee.F0601161_ANPA.ToString(), employee.F0601161_AN8.ToString() };
            }
            return new string[] { employee.F0601161_ANPA.ToString() };
        }
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
        public async Task SendMessage(int userId, string username, string message)
        {
            var user = userLookup[Context.ConnectionId];
            await Clients
                .Groups(user.Groups)
                .SendAsync("ReceiveMessage", ++MessageId, userId, username, message);
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
                var groups = new List<string>(GetGroups(userId));
                // maintain a lookup of connectionId-to-username
                userLookup.Add(currentId, new User { Id = userId, Name = userName, Groups = groups });
                // re-use existing message for now
                //await Clients.AllExcept(currentId).SendAsync("ReceiveMessage", userId, userName, $"{userName} joined the chat");
                groups.ForEach(async g => await Groups.AddToGroupAsync(currentId, g));
                await Clients
                    .Groups(groups)
                    .SendAsync("ReceiveMessage", ++MessageId, userId, userName, $"{userName} joined the chat");
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
                await Clients
                    .Groups(user.Groups)
                    //.AllExcept(Context.ConnectionId)
                    .SendAsync("ReceiveMessage", ++MessageId, user.Id, user.Name, $"{user.Name} has left the chat");
            }
            await base.OnDisconnectedAsync(e);
        }

        public ChatHub(GlobalAppState globalAppState)
        {
            GlobalAppState = globalAppState;
        }
    }
}

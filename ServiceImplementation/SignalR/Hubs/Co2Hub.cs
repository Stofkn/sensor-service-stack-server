using System;
using Microsoft.AspNet.SignalR;

namespace ServiceImplementation.SignalR.Hubs
{
    public class Co2Hub : Hub
    {
        public static void Send(string name, double co, DateTime date)
        {
             // Call the broadcastMessage method to update clients.
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Co2Hub>();
            hubContext.Clients.All.broadcastMessage(name, co, date);
        }
    }
}
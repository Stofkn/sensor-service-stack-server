using System;
using Microsoft.AspNet.SignalR;

namespace ServiceImplementation.SignalR.Hubs
{
    public class HumidityHub : Hub
    {
        public static void Send (string name, double humidity, DateTime date)
        {
             // Call the broadcastMessage method to update clients.
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<HumidityHub>();
            hubContext.Clients.All.broadcastMessage(name, humidity, date);
        }
    }
}
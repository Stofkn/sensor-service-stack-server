using System;
using Microsoft.AspNet.SignalR;

namespace ServiceImplementation.SignalR.Hubs
{
    public class TemperatureHub : Hub
    {
        public static void Send(string name, double temperature, DateTime date)
        {
             // Call the broadcastMessage method to update clients.
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<TemperatureHub>();
            hubContext.Clients.All.broadcastMessage(name, temperature, date);
        }
    }
}
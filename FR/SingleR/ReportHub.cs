using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FR.SingleR
{
    public class ReportHub : Hub
    {

        public async Task setreport(string AppName, string ReportName, string Extension, string Parameters, string connectionId)
        {

            new RabbitMQ.Quen(AppName, ReportName, Extension, Parameters, connectionId);

            //await Clients.Client(connectionId).SendAsync("givereport", "rapor işleme alındı");
        }
        public override Task OnConnectedAsync()
        {
            Clients.Client(Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);

            return base.OnConnectedAsync();
        }
        //public async Task GetConnectionId()
        //{
        //    //var httpContext = this.Context.GetHttpContext();
        //    //var userId = httpContext.Request.Query["userId"];
        //    await Clients.Client(Context.ConnectionId).SendAsync("getConnectionId", Context.ConnectionId);

        //}
        //public string GetConnectionId()
        //{
        //    return Context.ConnectionId;
        //}

    }
}

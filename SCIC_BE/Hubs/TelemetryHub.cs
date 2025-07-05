using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SCIC_BE.Hubs;

public class TelemetryHub: Hub
{
    public async Task SendTelemetryToClients(string json)
    {
        await Clients.All.SendAsync("ReceiveTelemetry", json);
    }    
}
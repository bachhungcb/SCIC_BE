using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SCIC_BE.Hubs;

public class TelemetryHub: Hub
{
    public async Task SendTelemetry(string deiceId, string payload)
    {
        await Clients.All.SendAsync("ReceiveTelemetry", deiceId, payload);
    }    
}
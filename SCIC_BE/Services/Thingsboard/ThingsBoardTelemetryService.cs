using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCIC_BE.Hubs;
using SCIC_BE.Interfaces.IServices;

namespace SCIC_BE.Services.Thingsboard;

public class ThingsBoardTelemetryService : IHostedService
{
    private readonly IConfiguration _configuration;
    private ThingsBoardWebSocketClient _tbClient;
    private ThingsBoardAuthService _tbAuthService;
    private readonly IHubContext<TelemetryHub> _hubContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public ThingsBoardTelemetryService(
        IConfiguration configuration,
        ThingsBoardAuthService tbAuthService,
        IHubContext<TelemetryHub> hubContext,
        IServiceScopeFactory serviceScopeFactory)
    {
        _configuration = configuration;
        _tbAuthService = tbAuthService;
        _hubContext = hubContext;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting ThingsBoardTelemetryService...");
        try
        {
            var tbHost = _configuration["ThingsBoard:Host"];
            var deviceId = _configuration["ThingsBoard:DeviceId"];

            var token = await _tbAuthService.LoginToThinkBoard();
            Console.WriteLine($"Token received: {token.Substring(0, 10)}...");


            _tbClient = new ThingsBoardWebSocketClient(tbHost, token, deviceId, _hubContext, _serviceScopeFactory);
            await _tbClient.ConnectAndSubscribeAsync();

            Console.WriteLine("Connected and subscribed to ThingsBoard WebSocket.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in StartAsync: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // TODO: Đóng kết nối WebSocket nếu cần
        return Task.CompletedTask;
    }
}
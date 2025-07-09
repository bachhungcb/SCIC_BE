using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCIC_BE.Hubs;
using SCIC_BE.Interfaces.IServices;
using SCIC_BE.Repositories.AttendanceLogRepository;

public class ThingsBoardWebSocketClient
{
    private readonly string _thingsboardWsUrl;
    private readonly string _jwtToken;
    private readonly string _deviceId;
    private ClientWebSocket _webSocket;
    private readonly IHubContext<TelemetryHub> _hubContext;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public ThingsBoardWebSocketClient(  string thingsboardWsUrl,
                                        string jwtToken,
                                        string deviceId,
                                        IHubContext<TelemetryHub> hubContext,
                                        IServiceScopeFactory serviceScopeFactory
                                        )
    {
        _jwtToken = jwtToken;
        _thingsboardWsUrl = $"{thingsboardWsUrl.TrimEnd('/')}";
        _deviceId = deviceId;
        _webSocket = new ClientWebSocket();
        _serviceScopeFactory = serviceScopeFactory;
        _hubContext = hubContext;
    }

    public async Task ConnectAndSubscribeAsync()
    {
        var cleanToken = _jwtToken?.Trim().Replace("\r", "").Replace("\n", "");
        ;
        try
        {
            await _webSocket.ConnectAsync(new Uri(_thingsboardWsUrl), CancellationToken.None);


            // Gửi lệnh subscribe telemetry realtime
            if (_webSocket.State == WebSocketState.Open)
            {
                // Gửi authCmd trước
                var authCmd = JsonConvert.SerializeObject(new
                {
                    authCmd = new
                    {
                        cmdId = 0,
                        token = cleanToken
                    }
                });

                await SendMessageAsync(authCmd);

                // Chờ một chút để server phản hồi OK (có thể 100~300ms)
                await Task.Delay(100);

                // Gửi cmds sau khi xác thực xong
                var cmds = JsonConvert.SerializeObject(new
                {
                    cmds = new[]
                    {
                        new
                        {
                            entityType = "DEVICE",
                            entityId = _deviceId.Trim().Replace("\r", "").Replace("\n", ""),
                            scope = "LATEST_TELEMETRY",
                            cmdId = 10,
                            type = "TIMESERIES"
                        }
                    }
                });
                await SendMessageAsync(cmds);
            }
            else
            {
                Console.WriteLine("WebSocket is not open after ConnectAsync");
            }

            // Bắt đầu nhận dữ liệu
            _ = Task.Run(() => ReceiveMessagesAsync());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task SendMessageAsync(string message)
    {
        // Force UTF8 without BOM
        var bytes = new UTF8Encoding(false).GetBytes(message);
        var segment = new ArraySegment<byte>(bytes);

        await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }


    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[8192];

        while (_webSocket.State == WebSocketState.Open)
        {
            var ms = new MemoryStream();
            WebSocketReceiveResult result;

            do
            {
                result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("WebSocket closed by server.");
                    Console.WriteLine(
                        $"Close status: {result.CloseStatus}, Description: {result.CloseStatusDescription}");
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty,
                        CancellationToken.None);
                    return;
                }

                ms.Write(buffer, 0, result.Count);
            } while (!result.EndOfMessage);

            ms.Seek(0, SeekOrigin.Begin);
            string message = Encoding.UTF8.GetString(ms.ToArray());


            if (result.MessageType == WebSocketMessageType.Close)
            {
                Console.WriteLine("WebSocket closed by server.");
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                break;
            }

            Console.WriteLine("Received telemetry update:");
            Console.WriteLine(message);
            await _hubContext.Clients.All.SendAsync("ReceiveTelemetry", message);
            // TODO: Xử lý dữ liệu telemetry ở đây
            try
            {
                JObject obj = JObject.Parse(message);
                JToken token = obj["data"]["Attendance_check-in"][0][1];
                JObject parseInner = JObject.Parse(token.ToString());

                Guid userId = new Guid(parseInner["user_id"].ToString());
                Guid lectureId = new Guid(parseInner["lecturer_id"].ToString());
                string status = parseInner["Status"].ToString();
                Guid deviceId = new Guid(_deviceId);
                
                Console.WriteLine("DeviceId: " + _deviceId);
                Console.WriteLine("LecturerId: " + lectureId);
                Console.WriteLine("StudentId: " + userId);
                Console.WriteLine("Status: " + status);
                using var scope = _serviceScopeFactory.CreateScope();
                var attendanceLogService = scope.ServiceProvider.GetRequiredService<IAttendanceLogService>();
                var attendanceService = scope.ServiceProvider.GetRequiredService<IAttendanceService>();

                // TODO: xử lý các giá trị ở đây (ghi log, lưu DB, v.v.)
                await attendanceLogService.AddAttendanceLogAsync(lectureId, deviceId, status);
                await attendanceService.UpdateStudentAttendancAsync(deviceId, userId, status);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi phân tích JSON: " + ex.Message);
            }
        }
    }
}
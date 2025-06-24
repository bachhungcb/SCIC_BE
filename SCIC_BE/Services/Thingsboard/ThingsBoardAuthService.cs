using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SCIC_BE.Services.Thingsboard;

public class ThingsBoardAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ThingsBoardAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    private class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public async Task<string> LoginToThinkBoard()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient();

            var baseUrl = _configuration["BaseURL"];
            var url = $"{baseUrl}/api/auth/login";

            var requestBody = new
            {
                username = "mxngocqb@gmail.com",
                password = "Thingsboard1"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync(url, content);

            // Kiểm tra mã trạng thái HTTP
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to login. Status code: {response.StatusCode}, Response: {errorResponse}");
            }

            // Đọc và xử lý phản hồi JSON
            var returnData = await response.Content.ReadAsStringAsync();

            // Kiểm tra nếu phản hồi có dữ liệu hợp lệ
            if (string.IsNullOrEmpty(returnData))
            {
                throw new Exception("Empty response received from the server.");
            }

            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(returnData);

            if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                throw new Exception("Token is empty or not found in the response.");
            }

            // Return the token
            return loginResponse.Token;
        }
        catch (Exception ex)
        {
            // Log chi tiết lỗi
            Console.WriteLine($"Error during login: {ex.Message}");
            // Ném lại ngoại lệ với thông tin chi tiết hơn nếu cần
            throw new Exception($"Login failed: {ex.Message}");
        }
    }
}
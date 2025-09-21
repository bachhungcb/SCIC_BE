using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SCIC_BE.DTO.RcpDTOs;

namespace SCIC_BE.Services.Server
{
    public class RcpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public RcpService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        private class LoginResponse
        {
            public required string Token { get; set; }
            public required string RefreshToken { get; set; }
        }

        private async Task<string> LoginToThinkBoard()
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


        public async Task<string> SendRpcRequestAsync(RcpRequestDTO[] requestDtoArray)
        {
            var httpClient = _httpClientFactory.CreateClient();

            // Lấy token
            var token = await LoginToThinkBoard();
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Failed to obtain a valid token from LoginToThinkBoard.");
            }

            var baseUrl = _configuration["BaseURL"];
            List<string> responses = new List<string>();// Danh sách để lưu phản hồi từ các yêu cầu       
            foreach (var requestDto in requestDtoArray)// Lặp qua từng yêu cầu trong mảng
            {
                var url = $"{baseUrl}/api/rpc/oneway/{requestDto.DeviceId}";

                // Tạo body yêu cầu
                var requestBody = new
                {
                    method = requestDto.Method,
                    @params = requestDto.Params,
                    persistent = false,
                    timeout = 5000
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                // Thiết lập header
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Log headers của yêu cầu
                Console.WriteLine("Request Headers:");

                foreach (var header in httpClient.DefaultRequestHeaders)
                {
                    Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                Console.WriteLine("Body: ", requestBody);

                try
                {
                    var response = await httpClient.PostAsync(url, content);

                    // Log headers của phản hồi
                    Console.WriteLine("Response Headers:");
                    foreach (var header in response.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    // Kiểm tra mã trạng thái HTTP
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        throw new Exception($"RPC request failed. Status code: {response.StatusCode}, Error: {errorResponse}");
                    }

                    // Đọc nội dung phản hồi và trả về
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Response Body: " + responseContent);
                    responses.Add(responseContent);// Lưu phản hồi vào danh sách
                    //return responseContent;
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi và ném lỗi
                    Console.WriteLine($"Error occurred while sending RPC request: {ex.Message}");
                    throw;
                }
            }
            return JsonConvert.SerializeObject(responses);// Trả về danh sách phản hồi dưới dạng JSON array
        }
    }
}

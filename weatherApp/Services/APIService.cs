
using weatherApp.Models;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
 

namespace weatherApp.Services
{
    public class APIService
    {
         
        private readonly HttpClient _httpClient;
        private readonly ILogger<APIService> _logger;
        private readonly JsonSerializerOptions _serializerOptions;
        public APIService(HttpClient httpClient,ILogger<APIService> logger)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://liganos.azurewebsites.net/");
          

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            _logger = logger;
        }

        public async Task<APIResponse<bool>> Login(string email, string password)
        {
            try
            {
                var login = new Login()
                {
                    Username = email,
                    Password = password
                };

                var json = JsonSerializer.Serialize(login, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/apiaccounts/createtoken", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error sending HTTP request: {response.StatusCode}");
                    return new APIResponse<bool>
                    {
                        Message = $"Error sending HTTP request: {response.StatusCode}"
                    };
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TokenResponse>(jsonResult, _serializerOptions);

                await SecureStorage.SetAsync("AccessToken", result!.Token);
                await SecureStorage.SetAsync("UserId", result.UserId.ToString());
                await SecureStorage.SetAsync("UserName", result.UserName);

                return new APIResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Login error: {ex.Message}");
                return new APIResponse<bool> { Message = ex.Message };
            }
        }
        public async Task<HttpResponseMessage> PostRequest(string endpoint, object data)
         { 
             try
             {
                 var json = JsonSerializer.Serialize(data, _serializerOptions);
                 var content = new StringContent(json, Encoding.UTF8, "application/json");


                var token = await SecureStorage.GetAsync("AccessToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    _logger.LogWarning("No access token found. Ensure user is logged in.");
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                 _logger.LogInformation($"POST Request to: {endpoint}");
                 _logger.LogInformation($"Request Content: {json}");


                 var result = await _httpClient.PostAsync(endpoint, content);

                 _logger.LogInformation($"Response Status: {result.StatusCode}");

                 var responseContent = await result.Content.ReadAsStringAsync();
                 _logger.LogInformation($"Response Content: {responseContent}");

                 return result;
             }
             catch (Exception ex)
             {
                 _logger.LogError($"Error sending POST request to {endpoint}: {ex.Message}");
                 return new HttpResponseMessage(HttpStatusCode.BadRequest);
             }
         }
        public async Task<string> GetStringAsync(string endpoint)
        {
            try
            {

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine(content);
                
                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching data from {endpoint}: {ex.Message}");
                throw;
            }
        }
        public async Task<HttpResponseMessage> DeleteRequest(string endpoint)
        {
            try
            {
                return await _httpClient.DeleteAsync(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending DELETE request to {endpoint}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        public async Task<HttpResponseMessage> PutRequest(string endpoint, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                return await _httpClient.PutAsync(endpoint, content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending PUT request to {endpoint}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
using BlazorApp.Client.Server.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Server.Services
{
    public static class StatusMessage
    {
        public static string Message { get; set; }
    }
    
    public class DataService:IDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public DataService(IHttpClientFactory httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClient ??
                throw new System.ArgumentNullException(nameof(httpClient));
            _httpContextAccessor = httpContextAccessor ??
                throw new System.ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<IEnumerable<WeatherForecast>> GetClaims()
        {
            var httpClient = _httpClientFactory.CreateClient("APIClient");

            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            if (accessToken != null)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            }
            
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/weather");

            var response = await httpClient.SendAsync(
                request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var content = await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>
                ( await response.Content.ReadAsStreamAsync(), new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return content;
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized ||
                     response.StatusCode == HttpStatusCode.Forbidden)
            {
                StatusMessage.Message = response.StatusCode.ToString();
            }
            else if (!response.IsSuccessStatusCode)
            {
                StatusMessage.Message = response.Content.ToString();
            }

            return null;
        }
    }
}

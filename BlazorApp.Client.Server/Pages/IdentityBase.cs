using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
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
using BlazorApp.Client.Server.Services;
using Microsoft.AspNetCore.Authentication;
using BlazorApp.Client.Server.Data;

namespace BlazorApp.Client.Server.Pages
{
    public class IdentityBase : ComponentBase
    {
        
        [Inject]
        public IHttpClientFactory HttpClient { get; set; }

        [Inject]
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        [Inject] public NavigationManager NavigationManager { get; set; }
        
        [Inject] public IDataService DataService { get; set; }

        public string DisplayMessage { get; set; }
        
        public IEnumerable<WeatherForecast> forecasts { get; set; } = new List<WeatherForecast>();

        protected override async Task OnInitializedAsync()
        {
           await GetIdentity();
        }

        private async Task GetIdentity()
        {
            var unauthorized = HttpStatusCode.Unauthorized.ToString();
            var forbidden = HttpStatusCode.Forbidden.ToString();
            
            if (StatusMessage.Message == null)
            {
                forecasts = await DataService.GetClaims();
            }
            else if(StatusMessage.Message == unauthorized || StatusMessage.Message == forbidden)
            {
                NavigationManager.NavigateTo("/Account/AccessDenied");
            }
            else
            {
                DisplayMessage = StatusMessage.Message;
            }
        }
    }
}
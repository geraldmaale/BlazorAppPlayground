using BlazorApp.Client.Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Client.Server.Services
{
    public interface IDataService
    {
        Task<IEnumerable<WeatherForecast>> GetClaims();
    }
}

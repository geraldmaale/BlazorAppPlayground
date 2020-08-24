using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorApp.Client.Server.HttpHandlers
{
    public class BearerToken2Handler:DelegatingHandler
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IHttpClientFactory _httpClientFactory;

        public BearerToken2Handler(IHttpContextAccessor httpContextAccessor,
                   IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
            _httpClientFactory = httpClientFactory ??
                 throw new ArgumentNullException(nameof(httpClientFactory));
        }


       
    }
}

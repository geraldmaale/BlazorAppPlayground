using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorApp.Client.Server.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using BlazorApp.Client.Server.HttpHandlers;
using BlazorApp.Client.Server.Services;

namespace BlazorApp.Client.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Clear jwt mapping
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            // Identity Server Middleware for Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                // Add Cookies
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)

                // Add OpenIdConnect
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = "https://localhost:5500"; // Development
                        options.RequireHttpsMetadata = false;
                        options.ClientId = "playgroundclient";
                        options.ClientSecret = "49C1A7E1-0C79-4A89-A3D6-A37998FB86B0";

                        options.UsePkce = true;
                        options.ResponseType = "code id_token";

                        options.Scope.Add("email");
                        options.Scope.Add("phone");
                        options.Scope.Add("offline_access");
                        options.Scope.Add("roles");
                        // options.Scope.Add("apiresource");
                        options.Scope.Add("api");

                        options.ClaimActions.DeleteClaim("sid");
                        options.ClaimActions.DeleteClaim("idp");
                        options.ClaimActions.DeleteClaim("s-hash");
                        options.ClaimActions.DeleteClaim("auth_time");

                        options.ClaimActions.MapUniqueJsonKey("role", "role");

                        options.SaveTokens = true;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = JwtClaimTypes.Name,
                            RoleClaimType = JwtClaimTypes.Role,
                        };

                    });

            // HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddHttpContextAccessor();

            // Bearer Token
            //services.AddTransient<BearerTokenHandler>();

            services.AddScoped<IDataService, DataService>();

            // HttpClient
            services.AddScoped<HttpClient>(s =>
            {
                var client = new HttpClient { BaseAddress = new Uri("https://localhost:44386/") };
                return client;
            });

            // create an HttpClient used for accessing the IDP
            services.AddHttpClient("IDPClient", c =>
            {
                c.BaseAddress = new Uri("https://localhost:5500/");
                c.Timeout = new TimeSpan(0, 0, 30);
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            // create an HttpClient used for accessing the API
            services.AddHttpClient("APIClient", c =>
            {
                c.BaseAddress = new Uri("https://localhost:44386/");
                c.Timeout = new TimeSpan(0, 0, 30);
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapControllers();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

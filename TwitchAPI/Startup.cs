using eTickets.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitchAPI.Data;
using TwitchAPI.Models.AppUsers;

namespace TwitchAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHttpClient();

            services.AddDbContext<TwitchContext>(opt => opt.UseSqlServer
                (Configuration.GetConnectionString("TwitchConnection")));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<ITwitchRepository, TwitchRepository>();

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TwitchContext>();
            services.AddMemoryCache();
            services.AddSession();

            // Default password parameters + google
            services.AddAuthentication(options => options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/account/google-login"; // Must be lowercase
                })
                .AddGoogle(options =>
                {
                    options.ClientId = "36441300999-09jqtrqm9kdgjg6ejmekorih7j9krkmb.apps.googleusercontent.com";
                    options.ClientSecret = "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";
                });

            //services.AddHttpClient("someClient", c =>
            //{
            //    c.BaseAddress = new Uri("https://someaddress.com/");
            //});
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
                app.UseExceptionHandler("/Home/Error");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Seed APP users
            AppUserInitializer.SeedUsersAndRolesAsync(app).Wait();
        }
    }
}

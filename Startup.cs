using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mtama.Data;
using Mtama.Models;
using Mtama.Scheduler;
using Mtama.Services;

namespace Mtama
{
    public class Startup
    {
        public static string WebRootPath = "";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // Add EF services to the services container.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "auth_cookie";
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.IsEssential = true;
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter; //"/Home/Index";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LogoutPath = "/Account/Logout";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.GetAppSetting("ApplicationSessionTimeout")));
                });
            services.AddAuthentication();
            //services.ConfigureApplicationCookie(options =>
            //{
                

            //    // Not creating a new object since ASP.NET Identity has created
            //    // one already and hooked to the OnValidatePrincipal event.
            //    // See https://github.com/aspnet/AspNetCore/blob/5a64688d8e192cacffda9440e8725c1ed41a30cf/src/Identity/src/Identity/IdentityServiceCollectionExtensions.cs#L56
            //    //   options.Events.OnRedirectToLogin = context =>
            //    //  {
            //    //      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    //      return Task.CompletedTask;
            //    //  };
            //});

            var csvFormatterOptions = new CsvFormatterOptions();
            services.AddMvc(options =>
            {
                options.OutputFormatters.Add(new CSVFormatter(csvFormatterOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", "text/csv");
            });

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IHostedService, ScheduleTask>();
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            WebRootPath = env.WebRootPath;

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

            //   Add Roles and Admin Account
            CreateAdminRole(serviceProvider).Wait();
        }

        private async Task CreateAdminRole(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "Super Admin", "Aggregator", "Farmer", "supplier" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            ApplicationUser superuser = await userManager.FindByEmailAsync("superadmin@rexmercury.com");
            if (superuser == null)
            {
                superuser = new ApplicationUser()
                {
                    UserName = "superadmin@rexmercury.com",
                    Email = "superadmin@rexmercury.com",
                    FirstName = "Super Admin",
                    AccessFailedCount = 0,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    Gender = "Male"
                };
                await userManager.CreateAsync(superuser, "Rex@1234");
                await userManager.AddToRoleAsync(superuser, "Super Admin");
            }
        }
    }
}

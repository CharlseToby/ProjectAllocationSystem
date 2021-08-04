using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectAllocationSystem.Data;
using ProjectAllocationSystem.Extensions;
using ProjectAllocationSystem.Models;
using ProjectAllocationSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAllocationSystem
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
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Parse connection URL to connection string for Npgsql
                string connectionString = Configuration["DATABASE_URL"];
                connectionString = connectionString.Replace("postgres://", string.Empty);

                var pgUserPass = connectionString.Split("@")[0];
                var pgHostPortDb = connectionString.Split("@")[1];
                var pgHostPort = pgHostPortDb.Split("/")[0];

                var pgDb = pgHostPortDb.Split("/")[1];
                var pgUser = pgUserPass.Split(":")[0];
                var pgPass = pgUserPass.Split(":")[1];
                var pgHost = pgHostPort.Split(":")[0];
                var pgPort = pgHostPort.Split(":")[1];

                connectionString = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}";
                //connectionString += ";sslmode=Require;Trust Server Certificate=true;";

                options.UseLazyLoadingProxies();
                options.UseNpgsql(connectionString);
            });

            services.AddHostedService<ProjectAllocator>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            #region Replacement for AddDefaultIdentity excluding DefaultUI
            services.AddMvc();
            //services.AddAuthentication(o =>
            //{
            //    o.DefaultScheme = IdentityConstants.ApplicationScheme;
            //    o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //})
            //.AddIdentityCookies(o => { });
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders()
               .AddSignInManager();
            services.AddTransient<IEmailSender, EmailSender>();
            #endregion
            
            services.AddControllersWithViews();

            services.AddScoped<AdminService>();
            services.AddScoped<Chat>();
            services.AddScoped<ProjectMatching>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager,
            AdminService adminService)
        {
            if(!Configuration.GetValue<bool>("DB_MIGRATED", true))
            {
                dbContext.Database.Migrate();
                userManager.SeedDatabase(adminService).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
                endpoints.MapRazorPages();
            });
        }
    }
}

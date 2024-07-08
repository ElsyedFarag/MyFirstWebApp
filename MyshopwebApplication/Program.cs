using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.Dbintializer;
using MyShop.DataAccess.Implemention;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Etuilities;
using Stripe;

namespace StartUpWebApllication.Myshop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDbintializer, Dbintializer>();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(4);
                options.SignIn.RequireConfirmedAccount = true;
            })
            .AddDefaultTokenProviders()
            .AddDefaultUI()
            .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddSingleton<IEmailSender, EmailSender>();
            builder.Services.Configure<StripeDetails>(builder.Configuration.GetSection("stripe"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
            SpeedUp();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Customer}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "admin",
                pattern: "{area=Admin}/{controller=Dashboard}/{action=Index}/{id?}");

            app.Run();
            void SpeedUp()
            {
                using(var scope = app.Services.CreateScope() )
                {
                    var dBintailizer = scope.ServiceProvider.GetRequiredService<IDbintializer>();
                    dBintailizer.Initialize();
                }
            }
        }
        
    }
}

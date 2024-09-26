using BKStore_MVC.Models;
using BKStore_MVC.Models.Context;
using BKStore_MVC.Repository;
using BKStore_MVC.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace BKStore_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequiredLength = 4;
                Options.Password.RequireDigit = false;
                Options.Password.RequireNonAlphanumeric = false;
                Options.Password.RequireUppercase = false;
            }).
                AddEntityFrameworkStores<BKstore_System>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
            builder.Services.AddScoped<IGovernorateRepository, GovernorateRepository>();
            builder.Services.AddScoped<ICountryRepository, CountryRepository>();
            builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IShippingRepository, ShippingRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();


            builder.Services.AddDbContext<BKstore_System>(Options => {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("BK1"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

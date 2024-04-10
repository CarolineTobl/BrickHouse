using BrickHouse.Data;
using BrickHouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BrickHouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<ScaffoldedDbContext>(options =>
                options.UseSqlServer(connectionString)); // Use the same connection string or a different one if required
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();


            // Configure HSTS
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365); // Adjust according to your requirements
            });


            //builder.Services.Configure<IdentityOptions>(options => { 
            //// Password settings.
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequiredUniqueChars = 1;
            //// Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;
            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //        "abcdefghijkImnopqrstuvwxyZABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -�_@+";
            //    options.User.RequireUniqueEmail = false;
            //}) ;
            //builder.Services.ConfigureApplicationCookie(options => { 
            //    // Cookie settings
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            //    //options.LoginPath = �/Identity/Account/Login�;
            //    //options.AccessDeniedPath = �/Identity/Account/AccessDenied�;
            //    options.SlidingExpiration = true;
            //}) ;


            builder.Services.AddControllersWithViews();

            //added
            builder.Services.AddScoped<IIntexRepository, EFIntexRepository>();

            //added
            builder.Services.AddRazorPages();

            //added
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

            //added
            builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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

            //added
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            app.MapControllerRoute("pagenumandcategory", "{primaryCategory}/{pageNum}", new { Controller = "Home", action = "Index" });
            app.MapControllerRoute("pagination", "{pageNum}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapControllerRoute("category", "{primaryCategory}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapDefaultControllerRoute();
            app.MapRazorPages();

            app.Run();
        }
    }
}

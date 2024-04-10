using BrickHouse.Data;
using BrickHouse.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// INTEX II
// Group 2-2
// Garrett Ashcroft, Jared Rosenlund, Vivian Solgere, and Caroline Tobler

namespace BrickHouse
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            
            // ASP.NET identity context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            
            // DbContext for business data (products, orders, etc.)
            builder.Services.AddDbContext<ScaffoldedDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Import identity package and related services
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            
            // Change default login path to use the codegenerator page
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login"; // Set the login path to the desired URL
            });

            // Configure HSTS
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
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

            // Services and stuff
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            
            // Creates an instance of the repository pattern for the session
            builder.Services.AddScoped<IIntexRepository, EFIntexRepository>();

            // Add instance of session cart and necessary tools
            builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            // Enable third-party auth through Google
            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Redirect from HTTP
            app.UseHttpsRedirection();

            // Add Content Security Policy
            //app.Use(async (context, next) =>
            //{
            //    // CSP directive string
            //    var csp = "default-src 'self'; " +
            //              "style-src 'self' 'https://fonts.googleapis.com'; " + // Allows styles from Google Fonts
            //              "font-src 'self' 'https://fonts.gstatic.com'; " + // Allows fonts to be loaded from Google Fonts
            //              "script-src 'self' " + // Allows scripts from your domain
            //              "'https://code.jquery.com' " + // Allows jQuery from code.jquery.com
            //              "'https://cdnjs.cloudflare.com' " + // Allows Popper.js from cdnjs
            //              "'https://stackpath.bootstrapcdn.com'; " + // Allows Bootstrap JavaScript from StackPath
            //                                                         // Add other directives as needed
            //              "";

            //    // Add the Content-Security-Policy header to the response
            //    context.Response.Headers.Add("Content-Security-Policy", csp);

            //    // Continue processing the request
            //    await next();
            //});

            // Enables static files in wwwroot folder
            app.UseStaticFiles();

            // Activate services
            app.UseSession();
            app.UseRouting();

            // Activate identity services
            app.UseAuthentication();
            app.UseAuthorization();

            // Routing and mapping
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );
            
            app.MapControllerRoute("pagenumandcategory", "{primaryCategory}/{pageNum}", new { Controller = "Home", action = "Index" });
            app.MapControllerRoute("pagination", "{pageNum}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapControllerRoute("category", "{primaryCategory}", new { Controller = "Home", action = "Index", pageNum = 1 });
            app.MapDefaultControllerRoute();
            
            // Let's do this!!
            app.Run();
        }
    }
}

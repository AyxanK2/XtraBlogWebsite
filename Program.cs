using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using XtraBlogWebsite.DAL;
using XtraBlogWebsite.Services;
namespace XtraBlogWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });
            builder.Services.AddScoped<LayoutService>();
            builder.Services.AddScoped<IEmailService,EmailServices>();
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.MapControllerRoute("blog-page", "esas-sehife", new { controller = "Home", Action = "Index" });
            app.MapControllerRoute("about-us", "haqqimizda", new { controller = "Home", Action = "About" });
            app.MapControllerRoute("contact-us", "elage", new { controller = "Home", Action = "Contact" });
            app.MapControllerRoute("post-single", "posts/{slug?}", new { Controller = "Post", Action = "Details" });

            app.MapControllerRoute(
                name: "admin",
                pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
          );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
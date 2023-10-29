using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolApplication.Data;
using SchoolApplication.Models;
using static System.Formats.Asn1.AsnWriter;
//using Hangfire;
//using Hangfire.SqlServer;
//using System.Configuration;
using SchoolApplication.Expire;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ObjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "AspNetCore.Identity.Application";
    options.LoginPath = "/Login/GeneralLogin"; // Set the login path
    options.AccessDeniedPath = "/Login/AccessDenied"; // Set the access denied path
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;

});



builder.Services.Configure<IdentityOptions>(options =>
{
    
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 5;
});


//builder.Services.AddScoped<CustomAuthenticationFilter>();

builder.Services.AddHangfire(x => 
{
    x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
    RecurringJob.AddOrUpdate<StudentService>(j => j.CheckAndDeleteExpiredStudents(), "30 17 * * *");

});

builder.Services.AddHangfireServer();

builder.Services.AddDistributedMemoryCache(); // Use an appropriate cache provider
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Set the session timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


//builder.Services.AddAuthentication().AddCookie(options => 
//{
//    //expire time for the authentication process
//    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
//    //every time a authenticated user makes a process the expire time is resetted
//    options.SlidingExpiration = true;
//});

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (!roleManager.RoleExistsAsync("Registerer").Result)
    {
        var roleResult = roleManager.CreateAsync(new IdentityRole("Registerer")).Result;
        if (!roleResult.Succeeded)
        {
            throw new Exception("Role called Registerer could not been instantiated!");
            // Handle role creation failure
        }
    }
    if(!roleManager.RoleExistsAsync("Student").Result)
    {
        var roleResult = roleManager.CreateAsync(new IdentityRole("Student")).Result;
        if (!roleResult.Succeeded)
        {
            throw new Exception("Role called Student could not been instantiated!");
            // Handle role creation failure
        }
    }
    if(!roleManager.RoleExistsAsync("Teacher").Result)
    {
        var roleResult = roleManager.CreateAsync(new IdentityRole("Teacher")).Result;
        if (!roleResult.Succeeded)
        {
            throw new Exception("Role called Teacher could not been instantiated!");
            // Handle role creation failure
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseStatusCodePages(async context =>
//{
//    var response = context.HttpContext.Response;

//    if (response.StatusCode == (int)HttpStatusCode.Unauthorized ||
//            response.StatusCode == (int)HttpStatusCode.Forbidden)
//        response.Redirect("/Login");
//});
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TokenTaking}/{id?}");

app.Run();

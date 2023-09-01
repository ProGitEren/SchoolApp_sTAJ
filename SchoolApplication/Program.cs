using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolApplication.Data;
using SchoolApplication.Models;
using static System.Formats.Asn1.AsnWriter;
//using Hangfire;
//using Hangfire.SqlServer;
//using System.Configuration;
//using SchoolApplication.Expire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options=>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 5;
});
//builder.Services.AddHangfireServer();

//builder.Services.AddScoped<StudentService>();

////HangFire These are to check every day a student expiretime is passing the current time and it will be doing the action CheckAndDeleteExpiredStudents
//// Add Hangfire services
//builder.Services.AddHangfire(configuration => configuration
//    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
//    .UseSimpleAssemblyNameTypeSerializer()
//    .UseRecommendedSerializerSettings()
//    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
//// Add the recurring job to check and delete expired students

//RecurringJob.AddOrUpdate<StudentService>(x => x.CheckAndDeleteExpiredStudents(), "30 17 * * *");

builder.Services.AddAuthentication().AddCookie(options => 
{
    //expire time for the authentication process
    options.ExpireTimeSpan = TimeSpan.FromSeconds(20);
    //every time a authenticated user makes a process the expire time is resetted
    options.SlidingExpiration = true;
});

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
  
app.UseHttpsRedirection();
app.UseStaticFiles();
//// UseHangfire middleware (before any middleware that might enqueue jobs)


//// ... (other middleware)

//// UseHangfireDashboard middleware (after app.UseHangfire)
//app.UseHangfireDashboard();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TokenTaking}/{id?}");

app.Run();

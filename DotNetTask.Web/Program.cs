using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DotNetTask.Data;
using DotNetTask.Services.Interfaces;
using DotNetTask.Services.Implementations;
using DotNetTask.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=../app.db"));
// options.UseInMemoryDatabase("DotNetTask"));

// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Services
builder.Services
    .AddScoped<ITaskService, TaskService>()
    .AddScoped<ILoginService, LoginService>()
    .AddScoped<IUserService, UserService>();

// Cookies config
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.None
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

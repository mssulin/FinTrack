using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/signin";
    options.AccessDeniedPath = "/access-denied";
});


// Repositories
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IIncomeRepository, IncomeRepository>();
builder.Services.AddScoped<ISavingRepository, SavingsRepository>();
builder.Services.AddScoped<ISavingHistoryRepository, SavingHistoryRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

// Services
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();
builder.Services.AddScoped<ISubService, SubService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ISavingService, SavingService>();

var app = builder.Build();

var sv = new CultureInfo("sv-SE");
CultureInfo.DefaultThreadCurrentCulture = sv;
CultureInfo.DefaultThreadCurrentUICulture = sv;

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(sv),
    SupportedCultures = new[] { sv },
    SupportedUICultures = new[] { sv }
};

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
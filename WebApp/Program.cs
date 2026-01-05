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
var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
var dataFolder = Path.Combine(appData, "FinTrackExam");
Directory.CreateDirectory(dataFolder);

var dbPath = Path.Combine(dataFolder, "exam.db");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

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
    options.LoginPath = "/Auth/SignIn";
    options.AccessDeniedPath = "/Auth/AccessDenied";
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

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
    
    var email = "demo@fintrack.se";
    var user = await userManager.FindByEmailAsync(email);

    if (user == null)
    {
        var newUser = new UserEntity
        {
            UserName = email,
            Email = email,
            FirstName = "Demo",
            LastName = "User"
        };
        
        await userManager.CreateAsync(newUser, "Demo123!");
    }
}

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

// Dev-only
if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        if (!(context.User?.Identity?.IsAuthenticated ?? false))
        {
            using var scope = context.RequestServices.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<UserEntity>>();

            var email = "demo@fintrack.se";
            var user = await userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
            }
        }

        await next();
    });
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
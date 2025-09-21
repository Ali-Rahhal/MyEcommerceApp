using ECApp.DataAccess.Data;
using ECApp.DataAccess.Repository;
using ECApp.DataAccess.Repository.IRepository;
using ECApp.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddAreaPageRoute("Customer", "/Index", "");
    });//adding support for razor pages and !!!Areas folder in project!!!

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));//to read stripe settings from appsettings.json and store in StripeSettings class in Utility

//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<AppDbContext>();//original line
builder.Services.AddIdentity<IdentityUser, IdentityRole>()//use AddIdentity to add roles
    .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();//AddDefaultTokenProviders is used because unlike
                                                                         //AddDefaultIdentity, AddIdentity does not add token providers by default

//adding the correct paths for login, logout and access denied//should be done after adding identity
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddDistributedMemoryCache();//to store session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);//how long the session will be stored in memory
    options.Cookie.HttpOnly = true;//to prevent client side script from accessing the cookie
    options.Cookie.IsEssential = true;//the cookie will be sent even if the user has not consented to non-essential cookies
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();//setting the api key for stripe

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();//should be added after UseRouting and before MapRazorPages

app.MapRazorPages();

app.Run();
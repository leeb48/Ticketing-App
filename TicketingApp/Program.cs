using Stripe;
using TicketingApp.Data;
using TicketingApp.Models;
using TicketingApp.Services.TicketLockService;

var builder = WebApplication.CreateBuilder(args);

// This port mapping may be needed during docker setup
// builder.WebHost.UseKestrel(options =>
// {
//     options.ListenAnyIP(int.Parse(builder.Configuration["AppPort"]!));
// });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TicketingAppCtx>();
builder.Services.AddScoped<ILockService<Ticket>, TicketLockService>();

StripeConfiguration.ApiKey = builder.Configuration["Stripe:ApiKey"];

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseHsts();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

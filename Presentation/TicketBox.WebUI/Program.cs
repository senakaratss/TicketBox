using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TicketBox.Application.Features.Categories.Queries;
using TicketBox.Application.Interfaces;
using TicketBox.Persistence.Context;
using TicketBox.Persistence.Identity;
using TicketBox.Persistence.Repositories;
using TicketBox.WebUI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<TicketContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<TicketContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = ("/Auth/Login");
});
builder.Services.AddMediatR(cfg =>cfg.RegisterServicesFromAssembly(typeof(GetCategoriesQuery).Assembly));

builder.Services.AddSignalR();
builder.Services.AddHostedService<SeatHoldCleanupService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IQrCodeService, QrCodeService>();
builder.Services.AddScoped<ITicketImageService, TicketImageService>();
builder.Services.AddScoped<IBookingEmailTemplate, BookingEmailTemplate>();

builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<SeatHub>("/seatHub");

app.Run();

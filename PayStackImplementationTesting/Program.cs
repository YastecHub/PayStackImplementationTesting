using Microsoft.EntityFrameworkCore;
using PayStackImplementationTesting.IPayStackServices;
using PayStackImplementationTesting.Models.Data;
using PayStackImplementationTesting.PayStackServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<PayStackService>(client =>
{
    client.BaseAddress = new Uri("https://api.paystack.co/");
    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {builder.Configuration["Paystack:SecretKey"]}");
});

builder.Services.AddScoped<IPayStackService, PayStackService>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

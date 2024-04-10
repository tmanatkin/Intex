using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Intex.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<IntexContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:IntexConnection"]);
});

// key vault stuff
builder.Services.AddScoped<IIntexRepository, EFIntexRepository>();
ConfigurationBuilder azureBuilder = new ConfigurationBuilder();
azureBuilder.AddAzureKeyVault(new Uri("https://IntexVault311.vault.azure.net/"), new DefaultAzureCredential());
IConfiguration configuration = azureBuilder.Build();
string mySecretValue = configuration["MySecret"];
Console.WriteLine($"My secret value is: {mySecretValue}");

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

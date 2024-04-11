using Azure.Identity;
using Azure.Security.KeyVault.Secrets;//

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Intex.Models;
using Intex.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// key vault
ConfigurationBuilder azureBuilder = new ConfigurationBuilder();
azureBuilder.AddAzureKeyVault(new Uri("https://IntexVault311.vault.azure.net/"), new DefaultAzureCredential());
IConfiguration configuration = azureBuilder.Build();
string connectionString = configuration["IntexConnectionString"];

// microsoft authenticator
var keyVaultUri = new Uri("https://IntexVault311.vault.azure.net/");
var client = new SecretClient(keyVaultUri, new DefaultAzureCredential());
KeyVaultSecret clientID = await client.GetSecretAsync("MicrosoftAuthClientId");
var ClientId = clientID.Value;
KeyVaultSecret clientSecret = await client.GetSecretAsync("MicrosoftAuthClientSecret");
var ClientSecret = clientSecret.Value;
// Console.WriteLine($"Client Secret: {ClientSecret}");
builder.Services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = ClientId;//"Authentication:Microsoft:ClientId"];
        microsoftOptions.ClientSecret = ClientSecret;//"Authentication:Microsoft:ClientSecret"];
    });

// identity tables
builder.Services.AddDbContext<IntexIdentityDbContext>(options => options.UseSqlServer(connectionString));

var dbContextOptions = new DbContextOptionsBuilder<IntexContext>()
    .UseSqlServer(connectionString)
    .Options;

builder.Services.AddSingleton(new IntexContext(dbContextOptions));

builder.Services.AddScoped<IIntexRepository, EFIntexRepository>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<IntexIdentityDbContext>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

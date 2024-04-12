using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
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

builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<IntexIdentityDbContext>();

// make better default passwords
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 12; // changed from default
    options.Password.RequiredUniqueChars = 1;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler(“/Home/Error”);
    app.UseDeveloperExceptionPage();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Add X-Content-Type-Options Header
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        await next();
    });
}

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

// Enable Content-Security-Policy (CSP) header
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline' https://code.jquery.com /js/bootstrap.js; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com /css/bootstrap.css; font-src 'self' https://fonts.gstatic.com data:; img-src 'self' data: https://m.media-amazon.com https://www.lego.com https://images.brickset.com https://www.brickeconomy.com; connect-src 'self';");
    await next();
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    var roles = new [] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using(var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
}

app.Run();

using LicenseeRecords.Web.Models;
using LicenseeRecords.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register JSON data services for Account and Product
builder.Services.AddSingleton<IJsonDataService<Account>>(provider =>
    new JsonDataService<Account>("/Users/surajsuresh/Desktop/Licensee Records/Data/Accounts.json"));
builder.Services.AddSingleton<IJsonDataService<Product>>(provider =>
    new JsonDataService<Product>("/Users/surajsuresh/Desktop/Licensee Records/Data/Products.json"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Index}/{id?}");

app.Run();
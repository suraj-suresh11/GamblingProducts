using LicenseeRecords.WebAPI.Models;
using LicenseeRecords.WebAPI.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<IJsonDataService<Account>>(provider =>
    new JsonDataService<Account>("Data/Accounts.json"));
builder.Services.AddSingleton<IJsonDataService<Product>>(provider =>
    new JsonDataService<Product>("Data/Products.json"));

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
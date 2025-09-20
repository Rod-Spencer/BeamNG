using Microsoft.AspNetCore.Components.Server;
using SpenSoft.DanBeamNG.Services;
using SpenSoft.EF.BeamNG;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(o => { o.DetailedErrors = true; });


builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });
builder.Services.AddScoped<BodyStyleDataService_Interface, BodyStyleDataService>();
builder.Services.AddScoped<ClassificationsDataService_Interface, ClassificationsDataService>();
builder.Services.AddScoped<ConfigurationDataService_Interface, ConfigurationDataService>();
builder.Services.AddScoped<CountriesDataService_Interface, CountriesDataService>();
builder.Services.AddScoped<DriveTrainDataService_Interface, DriveTrainDataService>();
builder.Services.AddScoped<ImagesDataDataService_Interface, ImagesDataDataService>();
builder.Services.AddScoped<VehicleDataService_Interface, VehicleDataService>();
builder.Services.AddScoped<VehicleImageDataService_Interface, VehicleImageDataService>();

builder.Services.AddSingleton<BeamNGContext>();
builder.Services.AddScoped<Browser_Service>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(44450, listenOptions =>
    {
        listenOptions.UseHttps(); // Uses default dev certificate
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseWebSockets();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
// This is a simple Blazor Server application setup.
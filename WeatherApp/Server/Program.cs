using WeatherApp.BusinessLogic.BusinessLogic;
using WeatherApp.BusinessLogic.IBusinessLogic;
using WeatherApp.Shared;
using Microsoft.AspNetCore.ResponseCompression;
using log4net.Config;
using log4net;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

ConfigureLogging();

ILog log = LogManager.GetLogger(typeof(Program));
log.Info("Blazor Server Application started.");



// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


builder.Services.AddScoped<IHome, HomeService>();
builder.Services.AddScoped<HttpClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
void ConfigureLogging()
{
    try
    {
        string baseDirectory = AppContext.BaseDirectory;

        string configPath = Path.Combine(baseDirectory, "Configuration", "log4net.config");

        if (!File.Exists(configPath))
        {
            Console.WriteLine($"Error: log4net configuration file not found at: {configPath}");
            return;
        }

        var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        XmlConfigurator.Configure(logRepository, new FileInfo(configPath));

        Console.WriteLine($"Logging initialized using config file: {configPath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error initializing logging: {ex.Message}");
    }
}
using Serilog;
using Lab2_Zakrasnianyi.Services;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
.MinimumLevel.Information()
.WriteTo.File(
"Logs/requests-.log",
rollingInterval: RollingInterval.Day,
outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} {Message:lj}{NewLine}")
.CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddLocalization(options =>
options.ResourcesPath = "Resources");

builder.Services
.AddControllersWithViews()
.AddViewLocalization()
.AddDataAnnotationsLocalization();

var emailSettings = builder.Configuration
.GetSection("EmailSettings")
.Get<EmailSettings>() ?? new EmailSettings();

builder.Services.AddSingleton(emailSettings);
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Localization settings.
var supportedCultures = new[] { "uk", "en" };

var localizationOptions = new RequestLocalizationOptions()
.SetDefaultCulture("uk")
.AddSupportedCultures(supportedCultures)
.AddSupportedUICultures(supportedCultures);



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var requestTime = DateTime.Now;
    var request = context.Request;

    var fullUrl =
    $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";

    var ipAddress =
    context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    Log.Information(
    "URL: {FullUrl} | Time: {RequestTime:yyyy-MM-dd HH:mm:ss} | IP: {IpAddress}",
    fullUrl,
    requestTime,
    ipAddress);

    await next();
});

// Enable localization.
app.UseRequestLocalization(localizationOptions);

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}")
.WithStaticAssets();

app.Run();
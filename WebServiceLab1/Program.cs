using Serilog;
using Lab2_Zakrasnianyi.Services;
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
builder.Services.AddControllersWithViews();
var emailSettings = builder.Configuration
.GetSection("EmailSettings")
.Get<EmailSettings>() ?? new EmailSettings();

builder.Services.AddSingleton(emailSettings);
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    var requestTime = DateTime.Now;
    var request = context.Request;

    var fullUrl = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

    Log.Information(
    "URL: {FullUrl} | Time: {RequestTime:yyyy-MM-dd HH:mm:ss} | IP: {IpAddress}",
    fullUrl,
    requestTime,
    ipAddress);

    await next();
});
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

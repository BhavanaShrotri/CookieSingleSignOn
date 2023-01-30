using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(
  new ConfigurationOptions
  {
      EndPoints = { "127.0.0.1:6379" },
  });

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"c:\dataprotection-persistkeys"))
    //.PersistKeysToStackExchangeRedis(redis)
    .SetApplicationName("unique");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World.. :)");
app.MapGet("/protected", () => "Secret.. !").RequireAuthorization();

app.Run();
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
    .PersistKeysToStackExchangeRedis(redis)
    .SetApplicationName("Unique");


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthorization();
app.UseAuthorization();

app.MapGet("/", () => "Hello World.. :)");
app.MapGet("/protected", () => "Secret.. !").RequireAuthorization();

app.Run();

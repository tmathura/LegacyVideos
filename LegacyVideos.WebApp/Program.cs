using LegacyVideos.WebApp.Services;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

IConfiguration configuration = builder.Configuration;

var restClient = new RestClient(configuration.GetValue<string>("WebApi:ApiBaseUrl"));

builder.Services.AddSingleton<IWebAppClient, WebAppClient>(serviceProvider => new WebAppClient(restClient));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Movies/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Movies}/{action=Index}/{id?}");

app.Run();

using LegacyVideos.Core.Implementations;
using LegacyVideos.Core.Interfaces;
using LegacyVideos.Infrastructure.Implementations;
using LegacyVideos.Infrastructure.Interfaces;
using LegacyVideos.WebApi.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration configuration = builder.Configuration;

builder.Services.AddSingleton<IMovieDal, MovieDal>(serviceProvider => new MovieDal(configuration.GetValue<string>("Database:ConnectionString"))); ;
builder.Services.AddSingleton<IMovieBl, MovieBl>();
builder.Services.AddSingleton<ISeriesDal, SeriesDal>(serviceProvider => new SeriesDal(configuration.GetValue<string>("Database:ConnectionString"))); ;
builder.Services.AddSingleton<ISeriesBl, SeriesBl>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});

var repository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
var fileInfo = new FileInfo(@"log4net.config");
log4net.Config.XmlConfigurator.Configure(repository, fileInfo);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Legacy Videos WebApi V1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

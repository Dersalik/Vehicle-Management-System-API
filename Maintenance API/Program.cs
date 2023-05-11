using Maintenance_API.Data;
using Maintenance_API.Filters;
using Maintenance_API.Helpers;
using Maintenance_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Host.UseSerilog((context, loggerConfig) => {
    loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .Enrich.WithExceptionDetails()
    .Enrich.FromLogContext()
    .Enrich.With<ActivityEnricher>()
    .WriteTo.File("log.txt");

});
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var apiVersionDescriptionProvider = builder.Services.BuildServiceProvider()
  .GetService<IApiVersionDescriptionProvider>();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo { Title = "maintenance.API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

});

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MaintenanceDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddHttpClient("VehicleAPI", config =>
{
    config.BaseAddress = new Uri("https://localhost:7266");
    config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestVersion= new Version(1, 0);
    config.DefaultRequestHeaders.Clear();
}); 
builder.Services.AddScoped<IVehicleApiService, VehicleApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(C => C.SwaggerEndpoint("/swagger/v1/swagger.json", "maintenance.API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await using var scope = app.Services.CreateAsyncScope();
using var db = scope.ServiceProvider.GetService<MaintenanceDbContext>();
await EnsureDb(app.Services, app.Logger);
app.Run();

async Task EnsureDb(IServiceProvider services, Microsoft.Extensions.Logging.ILogger logger)
{
using var db = services.CreateScope().ServiceProvider.GetRequiredService<MaintenanceDbContext>();
if (db.Database.IsRelational())
{
logger.LogInformation("Ensuring database exists and is up to date at connection string '{connectionString}'", connectionString);
//await db.Database.EnsureCreatedAsync();
await db.Database.MigrateAsync();
}
}


// Make the implicit Program class public so test projects can access it
public partial class Program { }
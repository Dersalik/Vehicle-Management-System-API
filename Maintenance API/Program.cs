using Maintenance_API.Data;
using Maintenance_API.Helpers;
using Maintenance_API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MaintenanceDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddHttpClient("VehicleAPI", config =>
{
    config.BaseAddress = new Uri("https://localhost:7266/api/");
    config.Timeout = new TimeSpan(0, 0, 30);
    
    config.DefaultRequestHeaders.Clear();
}); 
builder.Services.AddScoped<IVehicleApiService, VehicleApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

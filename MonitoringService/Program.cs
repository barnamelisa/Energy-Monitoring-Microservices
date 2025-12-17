using MonitoringService.Config;

using Microsoft.EntityFrameworkCore;
using MonitoringService.Repositories;

using MonitoringService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 🔹 Swagger din OpenApiConfig
builder.Services.AddOpenApiDocumentation();

// 🔹 Security & RabbitMQ
builder.Services.AddJwtSecurity(builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration);

builder.Services.AddDbContext<MonitoringDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IHourlyConsumptionRepository, HourlyConsumptionRepository>();
builder.Services.AddScoped<ISyncDeviceRepository, SyncDeviceRepository>();

builder.Services.AddScoped<MeasurementProcessingService>();
builder.Services.AddScoped<MonitoringService.Services.MonitoringService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Xolit.API.Reservation.Persistence;
using Xolit.API.Reservation.Application;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
ConfigureServices(builder);

var app = builder.Build();

// Configurar middlewares
ConfigureMiddlewares(app);

app.Run();

// Método para configurar servicios
void ConfigureServices(WebApplicationBuilder builder)
{
    // Agregar soporte para controladores
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // Configuración para ignorar ciclos de referencia
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });

    // Configurar HealthChecks con soporte para MySQL
    builder.Services.AddHealthChecks()
        .AddMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"), // Conexión a MySQL
            name: "MySQL",
            failureStatus: HealthStatus.Unhealthy,
            timeout: TimeSpan.FromSeconds(5))
        .AddCheck("API Health Check", () => HealthCheckResult.Healthy("API is running"));

    // Agregar Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Registrar servicios de Application y Persistence
    builder.Services.AddApplicationServices();
    builder.Services.AddPersistenceServices(builder.Configuration);

    // Configurar CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

// Método para configurar middlewares
void ConfigureMiddlewares(WebApplication app)
{
    // Configurar Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Real Estate API v1");
            options.RoutePrefix = string.Empty; // Swagger en la raíz
        });
    }

    // Usar CORS
    app.UseCors("CorsPolicy");

    // Usar redirección HTTPS
    app.UseHttpsRedirection();

    // Configurar autorización
    app.UseAuthorization();

    // Mapear controladores
    app.MapControllers();

    // Configurar HealthChecks
    app.UseHealthChecks("/health", new HealthCheckOptions
    {
        Predicate = _ => true,
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });

    // Agregar HealthChecks UI (opcional)
    app.MapHealthChecksUI(options =>
    {
        options.UIPath = "/health-ui";
    });
}

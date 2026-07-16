using GestaoPedidos.Api.Data;
using Microsoft.EntityFrameworkCore;
using GestaoPedidos.Api.Extensions;
using GestaoPedidos.Api.Middlewares;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

    Log.Information("Iniciando a aplicação");

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // Configuracao para logs
    builder.Host.UseSerilog((context, config) =>
    {
        config
     .ReadFrom.Configuration(context.Configuration)
     .WriteTo.Console(outputTemplate:
         "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
     .WriteTo.File("logs/gestaopedidos-.log", rollingInterval: RollingInterval.Day)
     .Enrich.FromLogContext();
    });

    builder.Services.AddDbContext<AppDbContext>(
        (options) => options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );


    builder.Services.AddRepositories();
    builder.Services.AddApplicationServices();

    builder.Services.AddControllers();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Tratamento básico de erros (try/catch ou middleware simples) 
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Seed do banco de dados no startup
    // escopo manual
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DbSeeder.SeedAsync(context);
    }


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Aplicação configurada, iniciando execução");
    app.Run();


}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
}
finally
{
    Log.CloseAndFlush();
}


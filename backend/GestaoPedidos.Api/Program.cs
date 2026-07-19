using GestaoPedidos.Api.Data;
using Microsoft.EntityFrameworkCore;
using GestaoPedidos.Api.Extensions;
using GestaoPedidos.Api.Middlewares;
using Serilog;
using Scalar.AspNetCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;
using GestaoPedidos.Api.OpenApi;

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

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );

    // FluentValidation
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();
    builder.Services.AddFluentValidationAutoValidation();

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi(options =>
    {
        options.AddSchemaTransformer<EnumSchemaTransformer>();
    });


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("FrontendDev", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Tratamento básico de erros (try/catch ou middleware simples) 
    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Aplica migrations e seed do banco de dados no startup
    // escopo manual
    if (!app.Environment.IsEnvironment("Testing"))
    {
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Em ambientes containerizados o Postgres pode ainda estar de pé quando o app inicia,
            // mesmo com o healthcheck do compose; algumas tentativas evitam falha na primeira subida.
            const int tentativasMigracao = 5;
            for (var tentativa = 1; tentativa <= tentativasMigracao; tentativa++)
            {
                try
                {
                    await context.Database.MigrateAsync();
                    break;
                }
                catch (Exception ex) when (tentativa < tentativasMigracao)
                {
                    Log.Warning(ex, "Falha ao aplicar migrations (tentativa {Tentativa}/{Total})", tentativa, tentativasMigracao);
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }

            await DbSeeder.SeedAsync(context);
        }
    }


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();

        //Swagger UI
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "GestaoPedidos API v1");
            options.RoutePrefix = "swagger";
        });

        // scalar
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();
    
    app.UseCors("FrontendDev");

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

// Expõe a classe Program (gerada implicitamente) para o projeto de testes.
public partial class Program { }
using GestaoPedidos.Api.Data;
using Microsoft.EntityFrameworkCore;
using GestaoPedidos.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(
    (options) => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);


builder.Services.AddRepositores();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed do banco de dados no startup
// escopo manual
using (var scope = app.Services.CreateScope()){
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

app.Run();

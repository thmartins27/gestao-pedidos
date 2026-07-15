using GestaoPedidos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // verifica se tem produtos ja cadastrados
        if (await context.Produtos.AnyAsync() || await context.Clientes.AnyAsync())
            return;

        // Seed Clientes
        var clientes = new List<Cliente>
        {
            new ("Maria Silva", "maria.silva@email.com"),
            new ("João Souza", "joao.souza@email.com"),
            new ("Ana Oliveira", "ana.oliveira@email.com")
        };
        context.Clientes.AddRange(clientes); 
        

        // Seed Produtos
        var produtos = new List<Produto>
        {
            new ("Notebook Dell", 4500.00m, 20),
            new ("Mouse logitech", 120.00m, 50),
            new ("Teclado Mecânico", 350.00m, 30),
            new ("Cabo USB-C", 45.00m, 5),
            new ("Monitor 27\"", 1800.00m, 15)
        };
        context.Produtos.AddRange(produtos);

        await context.SaveChangesAsync();

    }
}
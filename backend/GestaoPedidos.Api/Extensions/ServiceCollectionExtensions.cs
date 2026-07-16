using GestaoPedidos.Api.Repositories;
using GestaoPedidos.Api.Services;

namespace GestaoPedidos.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories (this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IClienteService, ClienteService>();
        return services;
    }
}
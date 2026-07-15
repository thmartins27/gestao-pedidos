using GestaoPedidos.Api.Repositories;

namespace GestaoPedidos.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositores (this IServiceCollection services)
    {
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}
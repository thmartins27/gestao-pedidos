using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.Repositories;

public record TotaisPorStatus(StatusPedido Status, int Quantidade, decimal ValorTotal);

public interface IDashboardRepository
{
    Task<IReadOnlyList<TotaisPorStatus>> ObterTotaisPorStatusAsync(CancellationToken ct = default);
}

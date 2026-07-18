using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Models.Enums;
using GestaoPedidos.Api.Repositories;

namespace GestaoPedidos.Api.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _repository;

    public DashboardService(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardPedidoDto> ObterResumoAsync(CancellationToken ct = default)
    {
        var totais = await _repository.ObterTotaisPorStatusAsync(ct);

        return new DashboardPedidoDto
        {
            TotalPedidos = totais.Sum(t => t.Quantidade),
            ValorTotalPedidos = totais.Sum(t => t.ValorTotal),
            PedidosPorStatus = MapearTodosOsStatus(totais)
        };
    }

    // Status sem pedidos não aparecem no agrupamento, mas o consumidor espera os três sempre presentes.
    private static List<PedidoStatusDto> MapearTodosOsStatus(IReadOnlyList<TotaisPorStatus> totais)
        => Enum.GetValues<StatusPedido>()
            .Select(status =>
            {
                var grupo = totais.FirstOrDefault(t => t.Status == status);
                return new PedidoStatusDto
                {
                    Status = status,
                    Quantidade = grupo?.Quantidade ?? 0,
                    ValorTotal = grupo?.ValorTotal ?? 0
                };
            })
            .ToList();
}

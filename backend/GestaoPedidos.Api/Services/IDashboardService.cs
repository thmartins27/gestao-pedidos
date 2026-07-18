using GestaoPedidos.Api.DTOs.Pedidos;

namespace GestaoPedidos.Api.Services;

public interface IDashboardService
{
    Task<DashboardPedidoDto> ObterResumoAsync(CancellationToken ct = default);
}

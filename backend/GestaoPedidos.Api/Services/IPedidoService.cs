using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.Services;

public interface IPedidoService
{
    Task<PagedResult<PedidoResumoDto>> ObterTodosAsync(int page, int pageSize, StatusPedido? status, CancellationToken ct = default);
    Task<PedidoDto> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<PedidoDto> CriarAsync(CreatePedidoDto dto, CancellationToken ct = default);
    Task<PedidoDto> AtualizarStatusAsync(int id, UpdateStatusPedidoDto dto, CancellationToken ct = default);

}
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;

namespace GestaoPedidos.Api.Repositories;

public interface IPedidoRepository
{
    Task<PagedResult<Pedido>> ObterTodosAsync(int page, int pageSize, StatusPedido? status, CancellationToken ct = default);

    Task<Pedido?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<Pedido?> ObterParaAtualizacaoAsync(int id, CancellationToken ct = default);
    Task AdicionarAsync(Pedido pedido, CancellationToken ct = default);
    Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default);

}
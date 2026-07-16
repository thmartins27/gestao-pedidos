using GestaoPedidos.Api.DTOs.Produtos;
using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Api.Services;

public interface IProdutoService
{
    Task<PagedResult<ProdutoDto>> ObterTodosAsync(int page, int pageSize, CancellationToken ct = default);
    Task<ProdutoDto> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<ProdutoDto> CriarAsync (CreateProdutoDto dto, CancellationToken ct = default);
    Task<ProdutoDto> AtualizarAsync(int id, UpdateProdutoDto dto, CancellationToken ct = default);
    Task RemoverAsync(int iid, CancellationToken ct = default);
}
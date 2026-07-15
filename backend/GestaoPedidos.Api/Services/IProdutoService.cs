using GestaoPedidos.Api.DTOs.Produtos;

namespace GestaoPedidos.Api.Services;

public interface IProdutoService
{
    Task<List<ProdutoDto>> ObterTodosAsync(CancellationToken ct = default);
    Task<ProdutoDto> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<ProdutoDto> CriarAsync (CreateProdutoDto dto, CancellationToken ct = default);
    Task<ProdutoDto> AtualizarAsync(int id, UpdateProdutoDto dto, CancellationToken ct = default);
    Task RemoverAsync(int iid, CancellationToken ct = default);
}
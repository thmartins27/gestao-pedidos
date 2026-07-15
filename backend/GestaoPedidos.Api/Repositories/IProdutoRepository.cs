using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Api.Repositories;

public interface IProdutoRepository
{
    Task<List<Produto>> ObterTodosAsync(CancellationToken ct = default);
    Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task AdicionarAsync(Produto produto, CancellationToken ct = default);

    void Atualizar(Produto produto);
    void Remover(Produto produto);

    Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default);

}
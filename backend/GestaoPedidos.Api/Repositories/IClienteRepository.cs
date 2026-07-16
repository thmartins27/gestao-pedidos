using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Api.Repositories;

public interface IClienteRepository
{
    Task<PagedResult<Cliente>> ObterTodosAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Cliente?> ObterPorIdAsync (int id, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, int? idExcluir = null, CancellationToken ct = default);
    Task AdicionarAsync(Cliente cliente, CancellationToken ct = default);
    void Atualizar(Cliente cliente);
    void Remover(Cliente cliente);

    Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default);
}
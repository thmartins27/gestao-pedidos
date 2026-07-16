using GestaoPedidos.Api.DTOs.Clientes;
using GestaoPedidos.Api.Models;

namespace GestaoPedidos.Api.Services;

public interface IClienteService
{
    Task<PagedResult<ClienteDto>> ObterTodosAsync (int page, int pageSize, CancellationToken ct = default);
    Task<ClienteDto> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<ClienteDto> CriarAsync(CreateClienteDto dto, CancellationToken ct = default);
    Task<ClienteDto> AtualizarAsync(int id, UpdateClienteDto dto, CancellationToken ct = default);
    Task RemoverAsync(int id, CancellationToken ct = default);

}
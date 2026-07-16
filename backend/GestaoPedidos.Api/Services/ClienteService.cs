using GestaoPedidos.Api.DTOs.Clientes;
using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Repositories;

namespace GestaoPedidos.Api.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }


    public async Task<PagedResult<ClienteDto>> ObterTodosAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var resultado = await _repository.ObterTodosAsync(page, pageSize, ct);

        return new PagedResult<ClienteDto>
        {
            Items = resultado.Items.Select(MapToDto).ToList(),
            Page = resultado.Page,
            PageSize = resultado.PageSize,
            TotalItems = resultado.TotalItems
        };
    }

    public async Task<ClienteDto> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var cliente = await _repository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException($"Cliente com id {id} não encontrado.");

        return MapToDto(cliente);
    }

    public async Task<ClienteDto> CriarAsync(CreateClienteDto dto, CancellationToken ct = default)
    {
        if (await _repository.ExisteEmailAsync(dto.Email, ct: ct))
            throw new ArgumentException($"Já existe um cliente cadastrado com o e-mail '{dto.Email}'.", nameof(dto.Email));

        var cliente = new Cliente(dto.Nome, dto.Email);

        await _repository.AdicionarAsync(cliente, ct);
        await _repository.SalvarAlteracoesAsync(ct);

        return MapToDto(cliente);
    }

    public async Task<ClienteDto> AtualizarAsync(int id, UpdateClienteDto dto, CancellationToken ct = default)
    {
        var cliente = await _repository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException($"Cliente com id {id} não encontrado.");

        if (await _repository.ExisteEmailAsync(dto.Email, id, ct))
            throw new ArgumentException($"Já existe um cliente cadastrado com o e-mail '{dto.Email}'.", nameof(dto.Email));

        cliente.Nome = dto.Nome;
        cliente.Email = dto.Email;

        _repository.Atualizar(cliente);
        await _repository.SalvarAlteracoesAsync(ct);

        return MapToDto(cliente);
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        var cliente = await _repository.ObterPorIdAsync(id, ct)
            ?? throw new NotFoundException($"Cliente com id {id} não encontrado.");

        _repository.Remover(cliente);
        await _repository.SalvarAlteracoesAsync(ct);
    }

    private static ClienteDto MapToDto(Cliente cliente) => new()
    {
        Id = cliente.Id,
        Nome = cliente.Nome,
        Email = cliente.Email
    };
}
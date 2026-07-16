using FluentValidation;
using GestaoPedidos.Api.Data;
using GestaoPedidos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Repositories;

public class ClienteRepository : IClienteRepository
{

    private readonly AppDbContext _context;
    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<PagedResult<Cliente>> ObterTodosAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Clientes.AsNoTracking().OrderBy(c => c.Nome);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);


        return new PagedResult<Cliente>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };
    }


    public async Task<Cliente?> ObterPorIdAsync (int id, CancellationToken ct = default)
        => await _context.Clientes.FindAsync([id], ct);

    public async Task<bool> ExisteEmailAsync (string email, int? idExcluir = null, CancellationToken ct = default)
        => await _context.Clientes
            .AsNoTracking()
            .AnyAsync(c => c.Email == email && (idExcluir == null || c.Id != idExcluir), ct);

    public async Task AdicionarAsync(Cliente cliente, CancellationToken ct = default)
        => await _context.Clientes.AddAsync(cliente);

    public void Atualizar(Cliente cliente)
        => _context.Clientes.Update(cliente);

    public void Remover(Cliente cliente)
        => _context.Clientes.Remove(cliente);

    public async Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct) > 0;
}
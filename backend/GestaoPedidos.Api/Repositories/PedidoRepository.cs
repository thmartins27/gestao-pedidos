using GestaoPedidos.Api.Data;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Repositories;

public class PedidoRepository : IPedidoRepository
{

    private readonly AppDbContext _context;
    public PedidoRepository (AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Pedido>> ObterTodosAsync (int page, int pageSize, StatusPedido? status, CancellationToken ct = default)
    {
        var query = _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Cliente)
            .AsQueryable();

        if(status.HasValue)
            query = query.Where(p => p.Status == status.Value);


        query = query.OrderByDescending(p => p.DataPedido);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Pedido>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };
    }

    public async Task<Pedido?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => await _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Pedido?> ObterParaAtualizacaoAsync(int id, CancellationToken ct = default)
        => await _context.Pedidos.FindAsync([id], ct);

    public async Task AdicionarAsync(Pedido pedido, CancellationToken ct = default)
        => await _context.Pedidos.AddAsync(pedido, ct);

    public async Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct) > 0;


}
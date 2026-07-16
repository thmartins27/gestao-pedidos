using GestaoPedidos.Api.Data;
using GestaoPedidos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<Produto>> ObterTodosAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var query = _context.Produtos.AsNoTracking().OrderBy(p => p.Nome);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);


        return new PagedResult<Produto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total
        };
    }

    public async Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => await _context.Produtos.FindAsync([id], ct);

    public async Task<List<Produto>> ObterPorIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        return await _context.Produtos
            .Where(p => ids.Contains(p.Id))
            .ToListAsync(ct);
    }

    public async Task AdicionarAsync(Produto produto, CancellationToken ct = default)
        => await _context.Produtos.AddAsync(produto, ct);

    public void Atualizar(Produto produto)
        => _context.Produtos.Update(produto);

    public void Remover(Produto produto)
        => _context.Produtos.Remove(produto);

    public async Task<bool> SalvarAlteracoesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct) > 0;

}
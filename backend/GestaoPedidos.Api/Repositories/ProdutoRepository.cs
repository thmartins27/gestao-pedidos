using GestaoPedidos.Api.Data;
using GestaoPedidos.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Repositories;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository (AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> ObterTodosAsync (CancellationToken ct = default) 
        => await _context.Produtos.AsNoTracking().ToListAsync(ct);

    public async Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => await _context.Produtos.FindAsync([id], ct);

    public async Task AdicionarAsync(Produto produto, CancellationToken ct = default)
        => await _context.Produtos.AddAsync(produto, ct);

    public void Atualizar(Produto produto)
        => _context.Produtos.Update(produto);

    public void Remover(Produto produto)
        => _context.Produtos.Remove(produto);

    public async Task<bool> SalvarAlteracoesAsync (CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct) > 0;

}
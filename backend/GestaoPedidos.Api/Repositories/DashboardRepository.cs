using GestaoPedidos.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GestaoPedidos.Api.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TotaisPorStatus>> ObterTotaisPorStatusAsync(CancellationToken ct = default)
        => await _context.Pedidos
            .AsNoTracking()
            .GroupBy(p => p.Status)
            .Select(grupo => new TotaisPorStatus(
                grupo.Key,
                grupo.Count(),
                grupo.Sum(p => p.ValorTotal)
            ))
            .ToListAsync(ct);
}

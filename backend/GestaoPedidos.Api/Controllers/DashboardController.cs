using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna o total de pedidos, o valor total geral e a quantidade de pedidos por status
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<DashboardPedidoDto>> ObterResumo(CancellationToken ct = default)
    {
        var resumo = await _service.ObterResumoAsync(ct);
        return Ok(resumo);
    }
}

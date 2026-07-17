using GestaoPedidos.Api.DTOs.Pedidos;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Models.Enums;
using GestaoPedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidoController(IPedidoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os pedidos paginados
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="status"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<PedidoResumoDto>>> ObterTodos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] StatusPedido? status = null,
        CancellationToken ct = default
    )
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var resultado = await _service.ObterTodosAsync(page, pageSize, status, ct);
        return Ok(resultado);
    }

    /// <summary>
    /// Retorna um pedido, incluindo seus itens
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoDto>> ObterPorId(int id, CancellationToken ct = default)
    {
        var pedido = await _service.ObterPorIdAsync(id, ct);
        return Ok(pedido);
    }

    /// <summary>
    /// Cria um novo pedido, validando estoque e calculando o total
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<PedidoDto>> Criar(CreatePedidoDto dto, CancellationToken ct = default)
    {
        var pedido = await _service.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
    }

    /// <summary>
    /// Atualiza o status de um pedido
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<PedidoDto>> AtualizarStatus(
        int id, UpdateStatusPedidoDto dto, CancellationToken ct = default
    )
    {
        var pedido = await _service.AtualizarStatusAsync(id, dto, ct);
        return Ok(pedido);
    }



}
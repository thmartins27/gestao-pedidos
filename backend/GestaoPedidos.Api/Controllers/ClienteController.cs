using GestaoPedidos.Api.DTOs.Clientes;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service)
    {
        _service = service;
    }

    /// <summary>
    /// Retorna todos os dados de forma paginada
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ClienteDto>>> ObterTodos(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default
    )
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

        var resultado = await _service.ObterTodosAsync(page, pageSize, ct);
        return Ok(resultado);
    }

    /// <summary>
    /// Retorna cliente pelo Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> ObterPorId(int id, CancellationToken ct = default)
    {
        var cliente = await _service.ObterPorIdAsync(id, ct);
        return Ok(cliente);
    }


    /// <summary>
    /// Cria um novo cliente
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Criar(CreateClienteDto dto, CancellationToken ct = default)
    {
        var cliente = await _service.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
    }

    /// <summary>
    /// Atualizar cliente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Atualizar(int id, UpdateClienteDto dto, CancellationToken ct = default)
    {
        var cliente = await _service.AtualizarAsync(id, dto, ct);
        return Ok(cliente);
    }


    /// <summary>
    /// Remove um cliente
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct = default)
    {
        await _service.RemoverAsync(id, ct);
        return NoContent();
    }


}
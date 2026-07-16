using GestaoPedidos.Api.DTOs.Produtos;
using GestaoPedidos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GestaoPedidos.Api.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutoController(IProdutoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdutoDto>>> ObterTodos(CancellationToken ct = default)
    {
        var produtos = await _service.ObterTodosAsync(ct);
        return Ok(produtos);
    }



    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorId(int id, CancellationToken ct = default)
    {
        var produto = await _service.ObterPorIdAsync(id, ct);
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar(CreateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = await _service.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> Atualizar(int id, UpdateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = await _service.AtualizarAsync(id, dto, ct);
        return Ok(produto);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct = default)
    {
        await _service.RemoverAsync(id, ct);
        return NoContent();
    }

}
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

    /// <summary>
    /// Lista todos os produtos cadastrados.
    /// </summary>
    /// <param name="ct"></param>
    [HttpGet]
    public async Task<ActionResult<List<ProdutoDto>>> ObterTodos(CancellationToken ct = default)
    {
        var produtos = await _service.ObterTodosAsync(ct);
        return Ok(produtos);
    }


    /// <summary>
    /// Busca um produto específico pelo seu identificador.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> ObterPorId(int id, CancellationToken ct = default)
    {
        var produto = await _service.ObterPorIdAsync(id, ct);
        return Ok(produto);
    }

    /// <summary>
    /// Cria um novo produto
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    [HttpPost]
    public async Task<ActionResult<ProdutoDto>> Criar(CreateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = await _service.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = produto.Id }, produto);
    }


    /// <summary>
    /// Atualiza um produto
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoDto>> Atualizar(int id, UpdateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = await _service.AtualizarAsync(id, dto, ct);
        return Ok(produto);
    }

    /// <summary>
    /// Exclui um produto
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id, CancellationToken ct = default)
    {
        await _service.RemoverAsync(id, ct);
        return NoContent();
    }

}
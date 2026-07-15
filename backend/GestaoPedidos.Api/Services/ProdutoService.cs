using GestaoPedidos.Api.DTOs.Produtos;
using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Repositories;

namespace GestaoPedidos.Api.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ProdutoDto>> ObterTodosAsync(CancellationToken ct = default)
    {
        var produtos = await _repository.ObterTodosAsync(ct);
        return produtos.Select(MapearParaDto).ToList();
    }

    public async Task<ProdutoDto> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var produto = await _repository.ObterPorIdAsync(id, ct);
        if(produto is null)
            throw new RecursoNaoEncontradoException("Produto", id);
        return MapearParaDto(produto);
    }

    public async Task<ProdutoDto> CriarAsync (CreateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = new Produto(dto.Nome, dto.Preco, dto.EstoqueAtual);

        await _repository.AdicionarAsync(produto, ct);
        await _repository.SalvarAlteracoesAsync(ct);

        return MapearParaDto(produto);
    }

    public async Task<ProdutoDto> AtualizarAsync(int id, UpdateProdutoDto dto, CancellationToken ct = default)
    {
        var produto = await _repository.ObterPorIdAsync(id, ct);

        if(produto is null)
            throw new RecursoNaoEncontradoException("produto", id);

        produto.Nome = dto.Nome;
        produto.Preco = dto.Preco;

        await _repository.SalvarAlteracoesAsync(ct);

        return MapearParaDto(produto);
    }

    public async Task RemoverAsync(int id, CancellationToken ct = default)
    {
        var produto = await _repository.ObterPorIdAsync(id, ct);
        if(produto is null)
            throw new RecursoNaoEncontradoException("Produto", id);

        _repository.Remover(produto);
        await _repository.SalvarAlteracoesAsync(ct);
    }

    private static ProdutoDto MapearParaDto(Produto produto) => new()
    {
      Id = produto.Id,
      Nome = produto.Nome,
      Preco = produto.Preco,
      EstoqueAtual = produto.EstoqueAtual  
    };

}
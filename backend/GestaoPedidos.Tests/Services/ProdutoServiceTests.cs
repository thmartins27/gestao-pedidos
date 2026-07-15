using GestaoPedidos.Api.DTOs.Produtos;
using GestaoPedidos.Api.Exceptions;
using GestaoPedidos.Api.Models;
using GestaoPedidos.Api.Repositories;
using GestaoPedidos.Api.Services;
using Moq;

namespace GestaoPedidos.Tests.Services;

public class ProdutoServiceTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly ProdutoService _service;

    public ProdutoServiceTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _service = new ProdutoService(_repositoryMock.Object);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoProdutoExiste_RetornaDtoCorreto()
    {
        var produto = new Produto("Teclado", 150m, 30) { Id = 1 };
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(produto);

        var resultado = await _service.ObterPorIdAsync(1, TestContext.Current.CancellationToken);

        Assert.Equal(1, resultado.Id);
        Assert.Equal("Teclado", resultado.Nome);
        Assert.Equal(150m, resultado.Preco);
        Assert.Equal(30, resultado.EstoqueAtual);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoProdutoNaoExiste_LancaRecursoNaoEncontrado()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);

        await Assert.ThrowsAnyAsync<RecursoNaoEncontradoException>(
            () => _service.ObterPorIdAsync(99, TestContext.Current.CancellationToken)
        );
    }

    [Fact]
    public async Task CriarAsync_ComDadosValidos_AdicionaEPersiste()
    {
        var dto = new CreateProdutoDto
        {
            Nome = "Mouse",
            Preco = 80m,
            EstoqueAtual = 50
        };

        var resultado = await _service.CriarAsync(dto, TestContext.Current.CancellationToken);

        // vericando se dto de retorno reflete ao de criacao
        Assert.Equal("Mouse", resultado.Nome);
        Assert.Equal(80m, resultado.Preco);
        Assert.Equal(50, resultado.EstoqueAtual);

        // verificacao de interacao
        _repositoryMock.Verify(
            r => r.AdicionarAsync(It.IsAny<Produto>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
        _repositoryMock.Verify(
            r => r.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()),
            Times.Once()
        );
    }

    [Fact]
    public async Task ObterTodosAsync_RetornaTodosMapeadosParaDto()
    {
        var produtos = new List<Produto>
        {
            new("Teclado", 150m, 30) {Id = 1},
            new("Mouse", 80m, 50) {Id = 2}
        };

        _repositoryMock
            .Setup(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(produtos);

        var resultado = await _service.ObterTodosAsync(TestContext.Current.CancellationToken);

        Assert.Equal(2, resultado.Count);
        Assert.Equal("Teclado", resultado[0].Nome);
        Assert.Equal("Mouse", resultado[1].Nome);
    }

    [Fact]
    public async Task RemoverAsync_QuandoProdutoNaoExiste_LancaRecursoNaoEncontrado()
    {
        _repositoryMock
            .Setup(r => r.ObterPorIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Produto?)null);

        await Assert.ThrowsAsync<RecursoNaoEncontradoException>(
            () => _service.RemoverAsync(99, TestContext.Current.CancellationToken)
        );

        _repositoryMock.Verify(
            r => r.Remover(It.IsAny<Produto>()),
            Times.Never
        );
    }

}